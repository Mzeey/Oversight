using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class User
    {
        public int Id {get;set;}
        public string? FirstName {get;set;}
        public string? MiddleName {get;set;}
        public string? LastName {get;set;}
        public string? ProfileImageUrl {get;set;}
        public string? HashedPassword {get;set;}
        public string? Username {get;set;}
        public string? Email {get;set;}
        public string? FamilyStatus {get;set;}
        public int ResidentialStatusId {get;set;}
        public string? PhoneNumber {get;set;}
        public string? HouseAddress {get;set;}
        public bool IsActive {get;set;}
        /// <summary>
        /// This is the authority level a user has in the application and it is ranked as follows:
        /// 0 - Admin
        /// 1 - Sub_Admin
        /// 2 - Employees
        /// 3 - Resident
        /// </summary>
        /// <value></value>
        public byte StatusControl {get;set;}
        public DateTime DateCreated {get;set;}
        public DateTime DateModified {get;set;}

        //Foreign Key Link
        public ResidentialStatus ResidentStatus {get;set;}

        //Relations
        public ICollection<UsersSetting> UsersSettings {get;set;}
        public ICollection<OtpCode> OtpCodes {get;set;}
        public ICollection<TwoFA> TwoFAs {get;set;}
        public ICollection<Subscription> Subscriptions {get;set;}
        public ICollection<PaymentMethod> PaymentMethods {get;set;}
        public ICollection<Invoice> Invoices {get;set;}


        public User(){
            this.ResidentStatus = new ResidentialStatus();

            this.UsersSettings = new HashSet<UsersSetting>();
            this.OtpCodes = new HashSet<OtpCode>();
            this.TwoFAs = new HashSet<TwoFA>();
            this.Subscriptions = new HashSet<Subscription>();
            this.PaymentMethods = new HashSet<PaymentMethod>();
            this.Invoices = new HashSet<Invoice>();
        }

    }
}