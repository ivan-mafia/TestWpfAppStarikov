using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfAppStarikov.Models
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using Catel.Data;

    public class Client : ModelBase
    {
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public int Id
        {
            get { return GetValue<int>(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Register the Id property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int), null);

        /// <summary>
            /// Gets or sets the property value.
            /// </summary>
        public DateTime BirthDate
        {
            get { return GetValue<DateTime>(BirthDateProperty); }
            set { SetValue(BirthDateProperty, value); }
        }

        /// <summary>
        /// Register the BirthDate property so it is known in the class.
        /// </summary>
        public static readonly PropertyData BirthDateProperty = RegisterProperty("BirthDate", typeof(DateTime), null);

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName
        {
            get { return GetValue<string>(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        /// Register the FirstName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string), null);

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName
        {
            get { return GetValue<string>(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        /// <summary>
        /// Register the LastName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LastNameProperty = RegisterProperty("LastName", typeof(string), null);

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                var validMsg = (string)Application.Current.FindResource("FirstNameIsRequired");
                validationResults.Add(FieldValidationResult.CreateError("FirstName", validMsg));
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                var validMsg = (string)Application.Current.FindResource("LastNameIsRequired");
                validationResults.Add(FieldValidationResult.CreateError("LastName", validMsg));
            }
        }

        public override string ToString()
        {
            string fullName = string.Empty;

            if (!string.IsNullOrEmpty(FirstName))
            {
                fullName += FirstName;
            }

            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                fullName += " ";
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                fullName += LastName;
            }

            return fullName;
        }
    }
}
