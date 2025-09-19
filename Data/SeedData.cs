// File: Data/SeedData.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourismApp.Models;

namespace TourismApp.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var context = provider.GetRequiredService<ApplicationDbContext>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

            await context.Database.MigrateAsync();

            string[] roles = new[] { "Tourist", "Agency" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                {
                    await roleManager.CreateAsync(new IdentityRole(r));
                }
            }

            var agency = await userManager.FindByEmailAsync("agency@demo.com");
            if (agency == null)
            {
                agency = new ApplicationUser
                {
                    UserName = "agency@demo.com",
                    Email = "agency@demo.com",
                    EmailConfirmed = true
                };
                var created = await userManager.CreateAsync(agency, "Passw0rd!");
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(agency, "Agency");
                }
            }

            var tourist = await userManager.FindByEmailAsync("tourist@demo.com");
            if (tourist == null)
            {
                tourist = new ApplicationUser
                {
                    UserName = "tourist@demo.com",
                    Email = "tourist@demo.com",
                    EmailConfirmed = true
                };
                var created = await userManager.CreateAsync(tourist, "Passw0rd!");
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(tourist, "Tourist");
                }
            }

            if (agency != null && await context.AgencyProfiles.FirstOrDefaultAsync(a => a.UserId == agency.Id) == null)
            {
                context.AgencyProfiles.Add(new AgencyProfile
                {
                    UserId = agency.Id,
                    AgencyName = "Sunny Trails",
                    Description = "Day hikes and city tours.",
                    Website = "https://example.com"
                });
                await context.SaveChangesAsync();
            }

            if (tourist != null && await context.TouristProfiles.FirstOrDefaultAsync(t => t.UserId == tourist.Id) == null)
            {
                context.TouristProfiles.Add(new TouristProfile
                {
                    UserId = tourist.Id,
                    FullName = "Alex Explorer",
                    Country = "Finland"
                });
                await context.SaveChangesAsync();
            }

            if (!await context.Amenities.AnyAsync())
            {
                context.Amenities.AddRange(
                    new Amenity { Name = "Hotel Pickup" },
                    new Amenity { Name = "Meals Included" },
                    new Amenity { Name = "Guide" }
                );
                await context.SaveChangesAsync();
            }

            if (agency == null || tourist == null)
            {
                return;
            }

            var agencyProfile = await context.AgencyProfiles.FirstAsync(a => a.UserId == agency.Id);
            var amenities = await context.Amenities.ToListAsync();
            var primaryAmenityId = amenities.First().Id;
            var optionalAmenityIds = amenities.Select(a => a.Id).ToList();

            var seedPackages = new List<TourPackage>
            {
                new TourPackage
                {
                    Title = "Old Town Walking Tour",
                    Description = "3-hour guided walk through historic sites.",
                    DurationDays = 1,
                    Price = 49,
                    GroupSizeLimit = 20,
                    ImagePath = "/images/placeholder.jpg"
                },
                new TourPackage
                {
                    Title = "Coastal Serenity Cruise",
                    Description = "Half-day catamaran cruise with gourmet canapés and a sunset toast.",
                    DurationDays = 1,
                    Price = 189,
                    GroupSizeLimit = 16,
                    ImagePath = "/images/placeholder.jpg"
                },
                new TourPackage
                {
                    Title = "Mountain Air Retreat",
                    Description = "Two-day alpine immersion with mindful hikes, cabin lodging, and locally sourced meals.",
                    DurationDays = 2,
                    Price = 420,
                    GroupSizeLimit = 12,
                    ImagePath = "/images/placeholder.jpg"
                },
                new TourPackage
                {
                    Title = "Rainforest Discovery Trail",
                    Description = "Guided eco-trail into lush rainforest with wildlife spotting and conservation briefing.",
                    DurationDays = 1,
                    Price = 155,
                    GroupSizeLimit = 18,
                    ImagePath = "/images/placeholder.jpg"
                }
            };

            foreach (var sample in seedPackages)
            {
                if (await context.TourPackages.AnyAsync(tp => tp.Title == sample.Title))
                {
                    continue;
                }

                sample.AgencyProfileId = agencyProfile.Id;
                sample.TourDates.Add(new TourDate
                {
                    Date = DateTime.UtcNow.Date.AddDays(7),
                    Capacity = sample.GroupSizeLimit
                });
                sample.TourDates.Add(new TourDate
                {
                    Date = DateTime.UtcNow.Date.AddDays(21),
                    Capacity = sample.GroupSizeLimit
                });

                context.TourPackages.Add(sample);
                await context.SaveChangesAsync();

                context.TourPackageAmenities.Add(new TourPackageAmenity
                {
                    TourPackageId = sample.Id,
                    AmenityId = primaryAmenityId
                });

                foreach (var amenityId in optionalAmenityIds.Where(id => id != primaryAmenityId))
                {
                    context.TourPackageAmenities.Add(new TourPackageAmenity
                    {
                        TourPackageId = sample.Id,
                        AmenityId = amenityId
                    });
                }

                await context.SaveChangesAsync();

                if (!await context.Bookings.AnyAsync(b => b.TourDate.TourPackageId == sample.Id))
                {
                    var tourDate = sample.TourDates.First();
                    context.Bookings.Add(new Booking
                    {
                        UserId = tourist.Id,
                        TourDateId = tourDate.Id,
                        Participants = Math.Min(3, sample.GroupSizeLimit),
                        Status = BookingStatus.Confirmed,
                        PaymentStatus = PaymentStatus.Pending,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

