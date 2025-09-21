using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismApp.Data;
using TourismApp.Models.ViewModels;
using TourismApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TourismApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public HomeController(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<IActionResult> Index()
        {
            // Remove the automatic redirect to AgencyDashboard
            // We'll let the user navigate there manually after login

            var tours = await _ctx.TourPackages
                .Include(t => t.AgencyProfile)
                .OrderByDescending(t => t.Id)
                .Take(6)
                .ToListAsync();

            return View(tours);
        }

        [Authorize(Roles = "Agency")]
        public async Task<IActionResult> AgencyDashboard()
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var agency = await _ctx.AgencyProfiles
                .FirstOrDefaultAsync(a => a.UserId == uid);

            if (agency == null)
            {
                // Auto-create agency profile for existing users who don't have one
                agency = new AgencyProfile
                {
                    UserId = uid,
                    AgencyName = "My Agency",
                    Description = "Welcome to my agency! Please update this description."
                };
                _ctx.AgencyProfiles.Add(agency);
                await _ctx.SaveChangesAsync();
            }

            // Get agency tour packages
            var tourPackages = await _ctx.TourPackages
                .Where(t => t.AgencyProfileId == agency.Id)
                .ToListAsync();

            // Get bookings for agency tours
            var bookings = await _ctx.Bookings
                .Include(b => b.TourDate)
                .ThenInclude(td => td.TourPackage)
                .Where(b => tourPackages.Select(t => t.Id).Contains(b.TourDate.TourPackageId))
                .ToListAsync();

            // Calculate stats
            var totalTours = tourPackages.Count;
            var activeBookings = bookings.Count(b => b.Status != Models.BookingStatus.Cancelled);
            var totalRevenue = bookings
                .Where(b => b.PaymentStatus == Models.PaymentStatus.Paid)
                .Sum(b => b.Participants * b.TourDate.TourPackage.Price);
                
            var pendingReviews = bookings.Count(b => b.Status == Models.BookingStatus.Completed && b.Feedback == null);

            // Get recent bookings (last 5)
            var recentBookings = bookings
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .Select(b => new RecentBookingVM
                {
                    PackageTitle = b.TourDate.TourPackage.Title,
                    BookingDate = b.CreatedAt,
                    Participants = b.Participants,
                    Status = b.Status.ToString(),
                    Revenue = b.Participants * b.TourDate.TourPackage.Price
                })
                .ToList();

            var vm = new AgencyDashboardVM
            {
                AgencyName = agency.AgencyName,
                AgencyDescription = agency.Description ?? "",
                TotalTours = totalTours,
                ActiveBookings = activeBookings,
                TotalRevenue = totalRevenue,
                PendingReviews = pendingReviews,
                RecentBookings = recentBookings
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();

    }
}
