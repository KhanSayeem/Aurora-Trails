-- This SQL script creates AgencyProfile records for existing agency users that don't have one
INSERT INTO AgencyProfiles (UserId, AgencyName, Description)
SELECT
    u.Id,
    'My Agency',
    'Welcome to my agency! Please update this description.'
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Agency'
AND u.Id NOT IN (SELECT UserId FROM AgencyProfiles WHERE UserId IS NOT NULL);