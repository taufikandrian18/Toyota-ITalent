using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Interfaces;
using Talent.WebAdmin.UserSide.Models;
using System.Text.RegularExpressions;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideGuestLoginService : Controller
    {
        private readonly TalentContext DB;
        private readonly UserSideAuthService AuthService;
        private readonly IEmailService MailService;

        public UserSideGuestLoginService(TalentContext db, UserSideAuthService authService, IEmailService mailService)
        {
            this.DB = db;
            this.AuthService = authService;
            this.MailService = mailService;

        }

        public async Task<ActionResult<BaseResponse>> Login(GuestLoginModel model)
        {
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeEmail == model.Email && Q.ManpowerStatusType == "GuestAccount" );

            if (user == null)
            {
                var message = new Message();

                //message.En = "Incorrect username";
                //message.Id = "Username salah !";

                message.En = "Invalid Login!";
                message.Id = "Login Gagal !";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            if (user.CounterLogin > 3)
            {
                var message = new Message();

                message.En = "Unsuccessful login was more than 3 times, please reset your password!";
                message.Id = "Gagal login sudah lebih dari 3 kali, silahkan reset password anda!";

                user.CounterLogin++;
                await DB.SaveChangesAsync();

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            if (user.Status == "Upgraded") {
                var message = new Message();

                message.En = "Please login using TAM Passport!";
                message.Id = "Silahkan login menggunakan TAM Passport";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            if (user.IsSuspended) {
                var message = new Message();

                message.En = "Your account has been suspended";
                message.Id = "Akun anda telah terblokir";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            if (user.LastSeenAt.HasValue)
            {
                var diff = (DateTime.UtcNow.AddHours(7).Date - user.LastSeenAt.Value.Date).TotalDays;
                
                if (diff >= 1)
                {
                    user.LearningPoint = user.LearningPoint + 2;
                }
            }
            else {
                user.LearningPoint = user.LearningPoint + 2;
            }

            if (!PasswordHasher.Verify(model.Password, user.Password))
            {
                var message = new Message();

                //message.En = "Incorrect password";
                //message.Id = "Password salah !";

                message.En = "Invalid Login!";
                message.Id = "Login Gagal !";

                user.CounterLogin++;
                await DB.SaveChangesAsync();

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var MobileSSOClaim = new MobileTalentJWTModel
            {
                Aud = Guid.NewGuid(),
                Email = user.EmployeeEmail,
                EmployeeId = user.EmployeeId,
                Exp = 1,
                Expiration = DateTime.Now.AddYears(1),
                Iss = "Talent",
                IssuedAt = DateTime.Now,
                Unique_name = user.EmployeeUsername
            };

            model.Token = AuthService.CreateMobileToken(MobileSSOClaim);
            model.Password = null;
            user.CounterLogin = 0;
            await DB.SaveChangesAsync();
            return StatusCode(200, BaseResponse.ResponseOk(model));
        }

        public async Task<ActionResult<BaseResponse>> Logout(string employeeId)
        {
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeId == employeeId);
            if (user == null)
            {
                var message = new Message();

                message.En = "Incorrect userId";
                message.Id = "Username salah !";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }


            user.LastSeenAt = DateTime.Now;

            var fcmToken = await DB.UserFcmTokens.FirstOrDefaultAsync(Q => Q.EmployeeId == employeeId);
            fcmToken.Token = null;

            await DB.SaveChangesAsync();
            return StatusCode(200, BaseResponse.ResponseOk());
        }

        public async Task<ActionResult<BaseResponse>> Register(GuestRegisterModel model)
        {
            Employees employeeData = new Employees();
            EmployeePositionMappings mappings = new EmployeePositionMappings();
            var query = DB.Employees.Where(Q=> Q.EmployeeEmail == model.Email || Q.EmployeeUsername == model.Email).ToList();

            if (query.Count != 0) {
                var msg = new Message();
                msg.En = "Email already registered";
                msg.Id = "Email sudah terdaftar";
                return BaseResponse.BadRequest(null, msg);
            }

            query = DB.Employees.Where(Q => Q.Ktp == model.NIK).ToList();
            if (query.Count != 0)
            {
                var msg = new Message();
                msg.En = "NIK already registered";
                msg.Id = "NIK sudah terdaftar";
                return BaseResponse.BadRequest(null, msg);
            }

            query = DB.Employees.Where(Q => Q.EmployeePhone == model.PhoneNumber).ToList();
            if (query.Count != 0)
            {
                var msg = new Message();
                msg.En = "Phone number already registered";
                msg.Id = "Nomor telpon sudah terdaftar";
                return BaseResponse.BadRequest(null, msg);
            }

            if (!String.IsNullOrEmpty(model.GuestUserData.EmployeeID)) {
                query = DB.Employees.Where(Q => Q.EmployeeId == model.GuestUserData.EmployeeID).ToList();
                if (query.Count != 0)
                {
                    var msg = new Message();
                    msg.En = "Employee ID already registered";
                    msg.Id = "Employee ID sudah terdaftar";
                    return BaseResponse.BadRequest(null, msg);
                }
            }


            if (query.Count == 0)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(model.GuestUserData.EmployeeID))
                    {
                        employeeData.EmployeeId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        employeeData.EmployeeId = model.GuestUserData.EmployeeID;
                    }
                    employeeData.EmployeeName = model.Fullname;
                    employeeData.EmployeeEmail = model.Email;
                    employeeData.EmployeeUsername = model.Email;
                    employeeData.EmployeePhone = model.PhoneNumber;
                    employeeData.Password = PasswordHasher.Hash(model.Password);
                    employeeData.ManpowerStatusType = model.GuestUserType;
                    employeeData.Ktp = model.NIK;
                    employeeData.Status = "Unverified";
                    employeeData.AccountValid = DateTime.Now.AddMonths(1);
                    employeeData.LastUpdatedPasswordDate = DateTime.Now;
                    //employeeData.EmployeeId = Guid.NewGuid().ToString();
                    employeeData.CreatedAt = DateTime.Now;
                    employeeData.UpdatedAt = DateTime.Now;
                    employeeData.CreatedBy = "System";
                    employeeData.UpdatedBy = "System";
                    employeeData.EmployeeExperience = 0;
                    employeeData.LearningPoint = 2;
                    employeeData.TeachingPoint = 0;
                    employeeData.NIP = employeeData.EmployeeId;
                    employeeData.IsCoach = false;
                    employeeData.IsTeamLeader = false;
                    employeeData.AccountValid = DateTime.Now.AddMonths(1);
                    employeeData.IsRequestUpgrade = false;
                    employeeData.IsSuspended = false;
                    employeeData.PoistionNote = model.GuestUserData.PositionName;
                    employeeData.Nickname = model.Fullname;
                    employeeData.LastSeenAt = DateTime.Now;
                    employeeData.IsDataValidation = false;
                    employeeData.CounterLogin = 0;
                    DB.Employees.Add(employeeData);

                    var valPositionID = int.Parse(model.GuestUserData.PositionID);
                    mappings.PositionId = valPositionID;
                    employeeData.OutletId = model.GuestUserData.Outlet;
                    mappings.EmployeeId = employeeData.EmployeeId;
                    mappings.Information = model.GuestUserData.PositionName;
                    mappings.CreatedAt = DateTime.Now;
                    mappings.UpdatedAt = DateTime.Now;
                    DB.EmployeePositionMappings.Add(mappings);

                    await DB.SaveChangesAsync();
                }
                catch (Exception x)
                {
                    return StatusCode(500, BaseResponse.Error(null, x));
                }
            }

            else
            {
                var msg = new Message();
                msg.En = "User already registered";
                msg.Id = "User sudah terdaftar";
                return BaseResponse.BadRequest(null, msg);
            }
            return StatusCode(200, BaseResponse.ResponseOk(model));
        }

        public async Task<ActionResult<BaseResponse>> UpdatePassword(GuestChangePasswordModel model)
        {
            var existing = DB.Employees.Where(Q => Q.EmployeeEmail == model.Email).FirstOrDefault();

            if (existing != null)
            {

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasMiniMaxChars = new Regex(@".{8,12}");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!hasLowerChar.IsMatch(model.NewPassword))
                {
                    var msg = new Message();
                    msg.En = "Password should contain at least one lower case letter.";
                    msg.Id = "Kata sandi harus mengandung setidaknya satu huruf kecil";
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                   
                }
                else if (!hasUpperChar.IsMatch(model.NewPassword))
                {
                    var msg = new Message();
                    msg.En = "Password should contain at least one upper case letter.";
                    msg.Id = "Kata sandi harus mengandung setidaknya satu huruf besar";
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }
                else if (!hasMiniMaxChars.IsMatch(model.NewPassword))
                {
                    var msg = new Message();
                    msg.En = "Password should not be lesser than 8 or greater than 12 characters.";
                    msg.Id = "Kata sandi tidak boleh kurang dari 8 atau lebih dari 12 karakter.";
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }
                else if (!hasNumber.IsMatch(model.NewPassword))
                {
                    var msg = new Message();
                    msg.En = "Password should contain at least one numeric value.";
                    msg.Id = "Kata sandi harus mengandung setidaknya satu angka.";
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                //else if (!hasSymbols.IsMatch(model.NewPassword))
                //{
                //    var msg = new Message();
                //    msg.En = "Password should contain at least one special case character.";
                //    msg.Id = "Password anda salah.";
                //    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                //    ErrorMessage = "Password should contain at least one special case character.";
                //    return false;
                //}
                else
                {

                    if (!string.IsNullOrEmpty(model.OldPaassword) && !string.IsNullOrEmpty(existing.Password))
                    {
                        if (PasswordHasher.Verify(model.OldPaassword, existing.Password))
                        {
                            existing.Password = PasswordHasher.Hash(model.NewPassword);

                            await DB.SaveChangesAsync();
                        }
                        else
                        {
                            var msg = new Message();
                            msg.En = "Invalid password.";
                            msg.Id = "Password anda salah.";
                            return StatusCode(400, BaseResponse.BadRequest(null, msg));
                        }
                    }
                    else
                    {
                        var msg = new Message();
                        msg.En = "Client Failed to send old password atau new password.";
                        msg.Id = "Client tidak mengirimkan password lama atau password baru.";
                        return StatusCode(400, BaseResponse.BadRequest(null, msg));
                    }
                }
            }

            else
            {
                var msg = new Message();
                msg.En = "User not found";
                msg.Id = "User tidak ditemukan";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }
            return StatusCode(200, BaseResponse.ResponseOk());

        }

        public async Task<ActionResult<BaseResponse>> GuestForgetPasssword(string email)
        {
            try
            {
                var userData = await DB.Employees.Where(Q => Q.ManpowerStatusType == "GuestAccount" && Q.EmployeeEmail == email).FirstOrDefaultAsync();

                if (userData == null)
                {
                    var msg = new Message();
                    msg.En = "User not found";
                    msg.Id = "User tidak ditemukan";
                    return StatusCode(400, BaseResponse.BadRequest(msg));
                }

                userData.CounterLogin = 0;
                await DB.SaveChangesAsync();

                var pwd = PasswordHasher.GeneratePassword(6);
                userData.Password = PasswordHasher.Hash(pwd);
                await DB.SaveChangesAsync();

                var mailData = new UserSideSendMailModel();

                mailData.Tos = new List<string> {
                    userData.EmployeeEmail,
                };

                mailData.Subject = "Forget Password";
                mailData.Message = "Hello, "+ userData.EmployeeName +" here is your new password " + pwd;

                await MailService.SendEmailAsync(mailData);
                //return StatusCode(200, BaseResponse.ResponseOk(mailData));
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> RequestForgetPassword(string email)   
        {
            try
            {
                var userData = await DB.Employees.Where(Q => Q.ManpowerStatusType == "GuestAccount" && Q.EmployeeEmail == email).FirstOrDefaultAsync();

                if (userData == null)
                {
                    var msg = new Message();
                    msg.En = "User not found";
                    msg.Id = "User tidak ditemukan";
                    return StatusCode(400, BaseResponse.BadRequest(msg));
                }


                var mailData = new UserSideSendMailModel();

                mailData.Tos = new List<string> {
                    userData.EmployeeEmail,
                };

                mailData.Subject = "Request Forget Password";
                mailData.Message = "Hello, "+ userData.EmployeeName +" here is your link to reset password ";

                await MailService.SendEmailAsync(mailData);
                return StatusCode(200, BaseResponse.ResponseOk(mailData));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UserForgetPasssword(UserForgetPassword model)
        {

            if (String.IsNullOrWhiteSpace(model.EmployeeID))
            {
                var msg = new Message();
                msg.En = "Employee ID is required";
                msg.Id = "Employee ID tidak boleh kosong";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }

            if (String.IsNullOrWhiteSpace(model.Message))
            {
                var msg = new Message();
                msg.En = "Message is required";
                msg.Id = "Message tidak boleh kosong";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }

            var mailValidator = new EmailAddressAttribute();
            if (!mailValidator.IsValid(model.Email))
            {
                var msg = new Message();
                msg.En = "Incorrect email address";
                msg.Id = "Format email anda salah";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }



            try
            {
                var userData = await DB.Employees.Where(Q => Q.ManpowerStatusType != "GuestAccount" && Q.EmployeeEmail == model.Email && Q.EmployeeId == model.EmployeeID).FirstOrDefaultAsync();

                if (userData == null)
                {
                    var msg = new Message();
                    msg.En = "User not found";
                    msg.Id = "User tidak ditemukan";
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                var mailData = new UserSideSendMailModel();

                mailData.Tos = new List<string> {
                 model.To
                };

                mailData.Ccs = model.CC;

                mailData.Subject = "Forget Password";
                mailData.Message = model.Message;

                await MailService.SendEmailAsync(mailData);
                return StatusCode(200, BaseResponse.ResponseOk(mailData));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> ContactUs(UserForgetPassword model)
        {

            if (String.IsNullOrWhiteSpace(model.EmployeeID))
            {
                var msg = new Message();
                msg.En = "Employee ID is required";
                msg.Id = "Employee ID tidak boleh kosong";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }

            if (String.IsNullOrWhiteSpace(model.Message))
            {
                var msg = new Message();
                msg.En = "Message is required";
                msg.Id = "Message tidak boleh kosong";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }

            var mailValidator = new EmailAddressAttribute();
            if (!mailValidator.IsValid(model.Email))
            {
                var msg = new Message();
                msg.En = "Incorrect email address";
                msg.Id = "Format email anda salah";
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }


            try
            {
                var userData = await DB.Employees.Where(Q => Q.EmployeeEmail == model.Email && Q.EmployeeId == model.EmployeeID).FirstOrDefaultAsync();

                if (userData == null)
                {
                    var msg = new Message();
                    msg.En = "User not found";
                    msg.Id = "User tidak ditemukan";
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                var mailData = new UserSideSendMailModel();

                mailData.Tos = new List<string> {
                 model.To
                };

                mailData.Ccs = model.CC;

                mailData.Subject = "iTalent Issues";
                mailData.Message = model.Message;

                await MailService.SendEmailAsync(mailData);
                return StatusCode(200, BaseResponse.ResponseOk(mailData));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetProfile(string Username)
        {
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeEmail == Username && Q.ManpowerStatusType == "GuestAccount");
            if (user == null)
            {
                var message = new Message();

                message.En = "Incorrect username";
                message.Id = "Username salah !";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }


            return StatusCode(200, BaseResponse.ResponseOk(user));
        }


        public async Task<ActionResult<BaseResponse>> CheckValidation(string EmployeeId)
        {
          
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeId == EmployeeId && Q.ManpowerStatusType == "GuestAccount");
            if (user == null)
            {
                var message = new Message();

                message.En = "Incorrect username";
                message.Id = "Username salah !";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            var diffDate = (user.AccountValid.Value.Date - DateTime.UtcNow.AddHours(7).Date).TotalDays;
            if (diffDate <= 1) {
                user.IsSuspended = true;
            }

            await DB.SaveChangesAsync();


            return StatusCode(200, BaseResponse.ResponseOk(user));
        }


        public async Task<ActionResult<BaseResponse>> GetAnnouncement(string Username)
        {
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeEmail == Username || Q.EmployeeUsername == Username || Q.EmployeeId == Username);
            if (user == null)
            {
                var message = new Message();

                message.En = "Incorrect username";
                message.Id = "Username salah !";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

       

            var now = DateTime.Now.Date;
            var diffDate = 0.0;
            var resp = new VMANnouncement();

            if (user.AccountValid.HasValue)
            {
                diffDate = (user.AccountValid.Value.Date - now).TotalDays;
                diffDate = Math.Round(diffDate);
                if (diffDate >= 30.00 && diffDate <= 31 && user.ManpowerStatusType == "GuestAccount" && user.Status != "Verified")
                {
                    resp.ExpTime30 = true;
                }
                else
                {
                    resp.ExpTime30 = false;
                }
            }
            else
            {
                resp.ExpTime30 = false;
            }

            if (user.AccountValid.HasValue)
            {
                diffDate = (user.AccountValid.Value.Date - now).TotalDays;
                diffDate = Math.Round(diffDate);
                if ((diffDate >= 7.00 && diffDate <= 7.99) &&  user.ManpowerStatusType == "GuestAccount" && user.Status != "Verified")
                {
                    resp.ExpTime7 = true;
                    resp.ExpTime30 = false;
                }
                else
                {
                    resp.ExpTime7 = false;
                }
            } else {
                resp.ExpTime7 = false;
            }

            resp.Announcement = true;

            if (user.LastSeenAt.HasValue)
            {
                var diff = (DateTime.UtcNow.AddHours(7).Date - user.LastSeenAt.Value.Date).TotalDays;

                if (diff >= 1)
                {
                    resp.LoginPoints = true;
                    user.LearningPoint = user.LearningPoint + 2;
                }
                else {
                    resp.LoginPoints = false;
                }
            }
            else
            {
                resp.LoginPoints = true;
            }

            if (user.ManpowerStatusType == "GuestAccount")
            {
                resp.ChangePswd = true;
            }
            else { 
                resp.ChangePswd = false;
            }

        

            if (user.LastUpdatedPasswordDate.HasValue)
            {
                diffDate = (now - user.LastUpdatedPasswordDate.Value.Date).TotalDays;
                diffDate = Math.Round(diffDate);
                if (diffDate >= 90.00 && diffDate <= 90.99 && user.ManpowerStatusType == "GuestAccount")
                {
                    resp.ChangePswd3M = true;
                }
                else
                {
                    resp.ChangePswd3M = false;
                }
            }
            else
            {
                resp.ChangePswd3M = false;
            }
            user.LastSeenAt = DateTime.Now;
            await DB.SaveChangesAsync();


            return StatusCode(200, BaseResponse.ResponseOk(resp));
        }

        public async Task<ActionResult<BaseResponse>> UserUpgradeAccount(GuestUpgradeAccountModel model)
        {
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeId == model.EmployeeID && Q.ManpowerStatusType != "Permanent");
            if (user == null)
            {
                var message = new Message();

                message.En = "User Not Found";
                message.Id = "User Tidak Ditemukan";

                return StatusCode(400, BaseResponse.BadRequest(msg: message, data: null));
            }

            user.ManpowerStatusType = "Permanent";
            user.Status = "Upgraded";
            user.OutletId = model.OutletID;

            var userMapping = await DB.EmployeePositionMappings.FirstOrDefaultAsync(Q => Q.EmployeeId == model.EmployeeID);
            if (userMapping == null)
            {
                var newUserMapping = new EmployeePositionMappings();
                newUserMapping.EmployeeId = model.EmployeeID;
                newUserMapping.PositionId = model.PositionID;
                newUserMapping.CreatedAt = DateTime.Now;
                await DB.AddAsync(newUserMapping);
            }
            else
            {
                userMapping.PositionId = model.PositionID;
            }

            await DB.SaveChangesAsync();

            return StatusCode(200, BaseResponse.ResponseOk(user));
        }


    }
}