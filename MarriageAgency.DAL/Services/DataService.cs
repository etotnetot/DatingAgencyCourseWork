﻿using Dapper;
using MarriageAgency.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MarriageAgency.DAL.Services
{
    public class DataService : IDataService
    {
        public const string ConnectionString = @"Data Source=DESKTOP-6HGNLK3;database=DataBase_MA;
                                            Connect Timeout=30; Integrated Security=SSPI";

        public IEnumerable<User> GetUsers()
        {
            var reqList = GetUserRequirements();
            var fetishList = GetUserFetishes();
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                SqlMapper.AddTypeHandler(new DapperRequirementTypeHandler());
                SqlMapper.AddTypeHandler(new DapperFetishTypeHandler());

                if (db.State == ConnectionState.Closed)
                    db.Open();

                var sqlExpression = @"SELECT c.ClientId, c.ClientFullName, c.ClientGender, c.Email, c.ClientPassword, c.EducationID, c.ZodiacSign,
                            c.BodyType, c.ClientCity, c.ClientKids, c.ClientHobbies, c.ClientInformation, c.RequirementID, 
                            c.ProfilePhoto, c.FetishID, c.BirthDate, p.RequirementID, p.PartnerGender, p.AgeFrom,
                            p.AgeTo, p.BodyType, p.Kids, p.Education, f.FetishID, f.FetishTitle
                            FROM Clients c 
                            INNER JOIN PartnerRequirements p ON c.RequirementID = p.RequirementID
                            INNER JOIN Fetishes f ON c.FetishID = f.FetishID";
                var users = db.Query<User, Requirement, Fetish, User>(sqlExpression, (user, requirement, fetish) => {
                    user.RequirementID = reqList.SingleOrDefault(req => req.RequirementID.Equals(requirement.RequirementID));

                    user.FetishID = fetishList.SingleOrDefault(fet => fet.FetishID.Equals(fetish.FetishID));

                    return user;
                }, splitOn: "RequirementID, FetishID").ToList();

                SqlMapper.ResetTypeHandlers();

                return users;
            }
        }

        public ZodiacSign GetZodiacByName(string name)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM ZodiacSigns WHERE ZodiacSignName = @ZodiacSignName";
                db.Open();
                var order = db.QuerySingleOrDefault<ZodiacSign>(query, new { ZodiacSignName = name });

                return order;
            }
        }

        public List<Invitation> GetInvitations()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                return db.Query<Invitation>("SELECT * FROM Invitations").ToList();
            }
        }

        public bool UpdateRequirements(Requirement requirementToAdd)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string updateQuery = @"UPDATE [dbo].[PartnerRequirements] SET PartnerGender = @PartnerGender,
                AgeFrom = @AgeFrom, AgeTo = @AgeTo, BodyType = @BodyType, Kids = @Kids, Education = @Education
                WHERE RequirementID = @RequirementID";

                var result = db.Execute(updateQuery, new
                {
                    requirementToAdd.RequirementID,
                    requirementToAdd.PartnerGender,
                    requirementToAdd.AgeFrom,
                    requirementToAdd.AgeTo,
                    requirementToAdd.BodyType,
                    requirementToAdd.Kids,
                    requirementToAdd.Education
                });
            }

            return true;
        }

        public bool UpdateCoupleStatus(int userId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string updateQuery = @"UPDATE Couples SET CoupleStatus = 'Married' WHERE FirstClient = @FirstClient OR SecondClient = @FirstClient";

                db.Execute(updateQuery, new { FirstClient = userId });
            }

            return true;
        }

        public bool DeleteCouple(int userId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string updateQuery = @"DELETE FROM Couples WHERE FirstClient = @FirstClient";

                db.Execute(updateQuery, new
                {
                    FirstClient = userId,
                });

                return true;
            }
        }

        public bool DeleteCoupleByID(int coupleId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string updateQuery = @"DELETE FROM Couples WHERE CoupleID = @CoupleID";
                db.Execute(updateQuery, new
                {
                    CoupleID = coupleId,
                });

                return true;
            }
        }

        public bool DeleteRequirements(Requirement requirementToDelete)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string updateQuery = @"DELETE FROM PartnerRequirements WHERE RequirementID = @RequirementID";

                db.Execute(updateQuery, new
                {
                    requirementToDelete.RequirementID,
                });
            }

            return true;
        }

        public List<Requirement> GetUserRequirements()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                return db.Query<Requirement>("SELECT * FROM PartnerRequirements").ToList();
            }
        }

        public List<Fetish> GetUserFetishes()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                return db.Query<Fetish>("SELECT * FROM Fetishes").ToList();
            }
        }

        public List<Invitation> GetUserInvitations()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                return db.Query<Invitation>("SELECT * FROM Invitations").ToList();
            }
        }

        public int GetLastRequirementId()
        {
            string sqlExpression = "SELECT TOP 1 * FROM PartnerRequirements ORDER BY RequirementID DESC";
            SqlConnection connection = new SqlConnection(ConnectionString);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);

            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                        return dt.Rows[0].Field<int>(0);
                    else
                        return 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public int GetLastFetishId()
        {
            string sqlExpression = "SELECT TOP 1 * FROM Fetishes ORDER BY FetishID DESC";
            SqlConnection connection = new SqlConnection(ConnectionString);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);

            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                        return dt.Rows[0].Field<int>(0);
                    else
                        return 0;
                }
            }
            catch
            {
                throw new Exception("Fetish was not found");
            }
        }

        public int GetLastInvitationId()
        {
            string sqlExpression = "SELECT TOP 1 * FROM Invitations ORDER BY InvitationID DESC";
            SqlConnection connection = new SqlConnection(ConnectionString);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);

            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                        return dt.Rows[0].Field<int>(0);
                    else
                        return 0;
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public int GetLastClientId()
        {
            string sqlExpression = "SELECT TOP 1 * FROM Clients ORDER BY ClientID DESC";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);

            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0].Field<int>(0);
                    else
                        return 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddRequirement(Requirement requirementToAdd)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string insertQuery = @"INSERT INTO [dbo].[PartnerRequirements]([PartnerGender], [AgeFrom], 
                                        [AgeTo], [BodyType], [Kids], [Education]) 
                                        VALUES (@PartnerGender, @AgeFrom, @AgeTo, @BodyType, @Kids, @Education)";

                db.Execute(insertQuery, new
                {
                    requirementToAdd.PartnerGender,
                    requirementToAdd.AgeFrom,
                    requirementToAdd.AgeTo,
                    requirementToAdd.BodyType,
                    requirementToAdd.Kids,
                    requirementToAdd.Education
                });
            }
        }

        public bool AddInvitation(Invitation invitationToSend)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string insertQuery = @"INSERT INTO [dbo].[Invitations] ([Sender], [Recipient], 
                                        [InvitationDate], [InvitationPlace]) 
                                        VALUES (@Sender, @Recipient, @InvitationDate, @InvitationPlace)";

                var result = db.Execute(insertQuery, new
                {
                    invitationToSend.Sender,
                    invitationToSend.Recipient,
                    invitationToSend.InvitationDate,
                    invitationToSend.InvitationPlace
                });

                return true;
            }
        }

        public bool AddFetish(Fetish fetishToAdd)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string insertQuery = @"INSERT INTO [dbo].[Fetishes]([FetishTitle]) 
                                        VALUES (@FetishTitle)";

                db.Execute(insertQuery, new
                {
                    fetishToAdd.FetishTitle
                });

                return true;
            }
        }

        public bool AddUser(User userToAdd)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string insertQuery = @"INSERT INTO [dbo].[Clients]([ClientFullName], [ClientGender], 
                                        [Email], [ClientPassword], [EducationID], [ZodiacSign], [BodyType],
                                        [ClientCity], [ClientKids], [ClientHobbies], [ClientInformation], [RequirementID],
                                        [ProfilePhoto], [FetishID], [BirthDate]) 
                                        VALUES (@ClientFullName, @ClientGender, @Email, @ClientPassword, @EducationID, @ZodiacSign,
                                                @BodyType, @ClientCity, @ClientKids, @ClientHobbies, @ClientInformation, @RequirementID, 
                                                @ProfilePhoto, @FetishID, @BirthDate)";

                var result = db.Execute(insertQuery, new
                {
                    userToAdd.ClientFullName,
                    userToAdd.ClientGender,
                    userToAdd.Email,
                    userToAdd.ClientPassword,
                    userToAdd.EducationID,
                    userToAdd.ZodiacSign,
                    userToAdd.BodyType,
                    userToAdd.ClientCity,
                    userToAdd.ClientKids,
                    userToAdd.ClientHobbies,
                    userToAdd.ClientInformation,
                    userToAdd.RequirementID.RequirementID,
                    userToAdd.ProfilePhoto,
                    userToAdd.FetishID.FetishID,
                    userToAdd.BirthDate
                });

                return true;
            }
        }

        public bool AuthorizeUser(string mail, string password)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            string currentQuery = $"SELECT * FROM Clients WHERE Email='{mail}' AND ClientPassword='{password}'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(currentQuery, sqlConnection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            return dataTable.Rows.Count == 1;
        }

        public bool AddCouple(User firstUser, User secondUser)
        {
            using (IDbConnection dataBase = new SqlConnection(ConnectionString))
            {
                string insertQuery = @"INSERT INTO [dbo].[Couples] ([FirstClient], [SecondClient], [CoupleStatus])
                                        VALUES (@FirstClient, @SecondClient, @CoupleStatus)";

                var result = dataBase.Execute(insertQuery, new
                {
                    FirstClient = firstUser.ClientID,
                    SecondClient = secondUser.ClientID,
                    CoupleStatus = "NotMarried"
                });

                return true;
            }
        }

        public List<Couple> GetCouples()
        {
            using (IDbConnection dataBase = new SqlConnection(ConnectionString))
            {
                if (dataBase.State == ConnectionState.Closed)
                    dataBase.Open();

                return dataBase.Query<Couple>("SELECT * FROM Couples").ToList();
            }
        }

        public bool DeleteInvitation(User userToDelete)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string deleteQuery = @"DELETE FROM Invitations WHERE Sender = @ClientID OR Recipient = @ClientID";

                db.Execute(deleteQuery, new
                {
                    ClientID = userToDelete.ClientID,
                });

                return true;
            }
        }

        public Couple GetUserCouple(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string currentQuery = "SELECT * FROM Couples WHERE FirstClient = @FirstClient";

                return db.QuerySingleOrDefault<Couple>(currentQuery, new { FirstClient = id });
            }
        }

        public Couple GetCoupleIfRelationship(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string currentQuery = "SELECT * FROM Couples WHERE SecondClient = @SecondClient";

                return db.QuerySingleOrDefault<Couple>(currentQuery, new { SecondClient = id });
            }
        }

        public bool DeleteUser(User userToDelete)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string updateQuery = @"DELETE FROM Clients WHERE ClientID = @ClientID";

                var result = db.Execute(updateQuery, new
                {
                    userToDelete.ClientID
                });

                return true;
            }
        }

        public bool UpdateUser(User userToUpdate)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string updateQuery = @"UPDATE [dbo].[Clients] SET ClientFullName = @ClientFullName, ClientPassword = @ClientPassword,
                                    EducationID = @EducationID, Email = @Email, ClientGender = @ClientGender, 
                                    ClientKids = @ClientKids, BodyType = @BodyType, ClientCity = @ClientCity,
                                    FetishID = @FetishID, ClientInformation = @ClientInformation, ClientHobbies = @ClientHobbies,
                                    ProfilePhoto = @ProfilePhoto
                                    WHERE 
                                    ClientID = @ClientID";

                var result = db.Execute(updateQuery, new
                {
                    userToUpdate.ClientFullName,
                    userToUpdate.ClientPassword,
                    userToUpdate.EducationID,
                    userToUpdate.Email,
                    userToUpdate.ClientGender,
                    userToUpdate.ClientKids,
                    userToUpdate.BodyType,
                    userToUpdate.ClientCity,
                    userToUpdate.FetishID.FetishID,
                    userToUpdate.ClientInformation,
                    userToUpdate.ClientHobbies,
                    userToUpdate.ClientID,
                    userToUpdate.ProfilePhoto
                });

                return true;
            }
        }
        public User GetUserByLogin(string loginOfUser)
        {
            string sql = $"SELECT * FROM Clients WHERE Email = @Email";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                return db.QuerySingleOrDefault<User>(sql, new { Email = loginOfUser });
            }
        }
    }

    public class DapperRequirementTypeHandler : SqlMapper.TypeHandler<Requirement>
    {
        public override void SetValue(IDbDataParameter parameter, Requirement value)
        {
            parameter.Value = Convert.ToInt32(value);
        }

        public override Requirement Parse(object value)
        {
            return new Requirement((int)value);
        }
    }

    public class DapperFetishTypeHandler : SqlMapper.TypeHandler<Fetish>
    {
        public override void SetValue(IDbDataParameter parameter, Fetish value)
        {
            parameter.Value = Convert.ToInt32(value);

        }

        public override Fetish Parse(object value)
        {
            return new Fetish((Int32)value);
        }
    }
}