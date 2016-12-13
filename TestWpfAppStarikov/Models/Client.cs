// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="Ivan">
//   Starikov Ivan, 2016.
// </copyright>
// <summary>
//   Defines the Client type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Catel.Data;

    /// <summary>
    /// The client.
    /// </summary>
    public class Client : ModelBase
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// The to string override method.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            string fullName = string.Empty;

            if (!string.IsNullOrEmpty(this.FirstName))
            {
                fullName += this.FirstName;
            }

            if (!string.IsNullOrEmpty(this.FirstName) && !string.IsNullOrWhiteSpace(this.LastName))
            {
                fullName += " ";
            }

            if (!string.IsNullOrWhiteSpace(this.LastName))
            {
                fullName += this.LastName;
            }

            return fullName;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// The validate fields.
        /// </summary>
        /// <param name="validationResults">
        /// The validation results.
        /// </param>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(this.FirstName))
            {
                var validMsg = (string)Application.Current.FindResource("FirstNameIsRequired");
                validationResults.Add(FieldValidationResult.CreateError("FirstName", validMsg));
            }

            if (string.IsNullOrWhiteSpace(this.LastName))
            {
                var validMsg = (string)Application.Current.FindResource("LastNameIsRequired");
                validationResults.Add(FieldValidationResult.CreateError("LastName", validMsg));
            }
        } 
        #endregion
    }
}
