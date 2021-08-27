﻿using System.ComponentModel.DataAnnotations;
using triggan.BlogModel;

namespace triggan.BlazorApp.Model
{
    [MetadataType(typeof(UserRegistrationMetadata))]
    public class RegisterModel : User
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public RegisterModel()
        {

        }

        public RegisterModel(User baseUser)
        {
            Id = baseUser.Id;
            Username = baseUser.Username;
            FirstName = baseUser.FirstName;
            LastName = baseUser.LastName;
            Email = baseUser.Email;
            Role = baseUser.Role;
        }
    }
}
