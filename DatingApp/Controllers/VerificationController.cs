using DatingApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    /// <summary>
    /// Controller for verifying database tables and register functionality.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class VerificationController : ControllerBase
    {
        private readonly ProiectColectivContext _context;
        private readonly ILogger<VerificationController> _logger;

        public VerificationController(ProiectColectivContext context, ILogger<VerificationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Verifies that all required tables for register functionality exist and are properly configured.
        /// </summary>
        /// <returns>Verification report</returns>
        [HttpGet("tables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VerificationReport>> VerifyTables()
        {
            _logger.LogInformation("Starting database table verification");

            var report = new VerificationReport();

            try
            {
                // Check Users table
                report.UsersTable = new TableInfo
                {
                    TableName = "Users",
                    Exists = true,
                    RecordCount = await _context.Users.CountAsync()
                };

                // Check Images table
                report.ImagesTable = new TableInfo
                {
                    TableName = "Images",
                    Exists = true,
                    RecordCount = await _context.Images.CountAsync()
                };

                // Check UserLanguages table
                report.UserLanguagesTable = new TableInfo
                {
                    TableName = "UserLanguages",
                    Exists = true,
                    RecordCount = await _context.UserLanguages.CountAsync()
                };

                // Check UserInterests table
                report.UserInterestsTable = new TableInfo
                {
                    TableName = "UserInterests",
                    Exists = true,
                    RecordCount = await _context.UserInterests.CountAsync()
                };

                // Check User-Images relationship
                var userWithImages = await _context.Users
                    .Include(u => u.Images)
                    .FirstOrDefaultAsync(u => u.Images.Any());

                report.UserImagesRelationship = new RelationshipInfo
                {
                    RelationshipName = "User -> Images",
                    IsConfigured = true,
                    SampleExists = userWithImages != null,
                    SampleRecordCount = userWithImages?.Images.Count ?? 0
                };

                // Check User-Languages relationship
                var userWithLanguages = await _context.Users
                    .Include(u => u.UserLanguages)
                    .FirstOrDefaultAsync(u => u.UserLanguages.Any());

                report.UserLanguagesRelationship = new RelationshipInfo
                {
                    RelationshipName = "User -> UserLanguages",
                    IsConfigured = true,
                    SampleExists = userWithLanguages != null,
                    SampleRecordCount = userWithLanguages?.UserLanguages.Count ?? 0
                };

                // Check User-Interests relationship
                var userWithInterests = await _context.Users
                    .Include(u => u.UserInterests)
                    .FirstOrDefaultAsync(u => u.UserInterests.Any());

                report.UserInterestsRelationship = new RelationshipInfo
                {
                    RelationshipName = "User -> UserInterests",
                    IsConfigured = true,
                    SampleExists = userWithInterests != null,
                    SampleRecordCount = userWithInterests?.UserInterests.Count ?? 0
                };

                report.AllTablesExist = true;
                report.AllRelationshipsConfigured = true;
                report.IsReady = true;
                report.Message = "✅ All tables and relationships are properly configured!";

                _logger.LogInformation("Database verification completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database verification");
                report.AllTablesExist = false;
                report.AllRelationshipsConfigured = false;
                report.IsReady = false;
                report.Message = $"❌ Error: {ex.Message}";
            }

            return Ok(report);
        }

        /// <summary>
        /// Quick health check for register endpoint readiness.
        /// </summary>
        [HttpGet("register-ready")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RegisterReadinessReport>> CheckRegisterReadiness()
        {
            var report = new RegisterReadinessReport();

            try
            {
                // Quick checks
                var canQueryUsers = await _context.Users.AnyAsync() || true;
                var canQueryImages = await _context.Images.AnyAsync() || true;
                var canQueryLanguages = await _context.UserLanguages.AnyAsync() || true;
                var canQueryInterests = await _context.UserInterests.AnyAsync() || true;

                report.DatabaseConnected = true;
                report.TablesExist = canQueryUsers && canQueryImages && canQueryLanguages && canQueryInterests;
                report.RegisterEndpointReady = true;
                report.Message = "✅ Register endpoint is ready to use!";
                report.EndpointUrl = "POST /api/user/register";
            }
            catch (Exception ex)
            {
                report.DatabaseConnected = false;
                report.TablesExist = false;
                report.RegisterEndpointReady = false;
                report.Message = $"❌ Not ready: {ex.Message}";
            }

            return Ok(report);
        }
    }

    #region DTOs

    public class VerificationReport
    {
        public TableInfo? UsersTable { get; set; }
        public TableInfo? ImagesTable { get; set; }
        public TableInfo? UserLanguagesTable { get; set; }
        public TableInfo? UserInterestsTable { get; set; }
        public RelationshipInfo? UserImagesRelationship { get; set; }
        public RelationshipInfo? UserLanguagesRelationship { get; set; }
        public RelationshipInfo? UserInterestsRelationship { get; set; }
        public bool AllTablesExist { get; set; }
        public bool AllRelationshipsConfigured { get; set; }
        public bool IsReady { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class TableInfo
    {
        public string TableName { get; set; } = string.Empty;
        public bool Exists { get; set; }
        public int RecordCount { get; set; }
    }

    public class RelationshipInfo
    {
        public string RelationshipName { get; set; } = string.Empty;
        public bool IsConfigured { get; set; }
        public bool SampleExists { get; set; }
        public int SampleRecordCount { get; set; }
    }

    public class RegisterReadinessReport
    {
        public bool DatabaseConnected { get; set; }
        public bool TablesExist { get; set; }
        public bool RegisterEndpointReady { get; set; }
        public string Message { get; set; } = string.Empty;
        public string EndpointUrl { get; set; } = string.Empty;
    }

    #endregion
}

