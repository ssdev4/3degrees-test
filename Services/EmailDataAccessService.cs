using System.Data;
using System.Data.SqlClient;
using TestApp3D.Models;

public class EmailDataAccesService : IEmailDataAccessService
{
    private readonly IConfiguration _configuration;


    public EmailDataAccesService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool SendEmail(Email email)
    {
        using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("msdb.dbo.sp_send_dbmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters required for the stored procedure
                    command.Parameters.AddWithValue("@profile_name", _configuration["SpSendDbMail:ProfileName"]);
                    command.Parameters.AddWithValue("@recipients", email.RecipientEmail);
                    command.Parameters.AddWithValue("@body", email.Body);
                    command.Parameters.AddWithValue("@subject", email.Subject);

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    return true; // Email sent successfully
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
