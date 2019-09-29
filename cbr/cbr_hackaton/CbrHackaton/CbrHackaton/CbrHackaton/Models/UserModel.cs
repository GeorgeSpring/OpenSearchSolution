using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace CbrHackaton.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        [JsonIgnore]
        public static string ComplexEmailPattern4 = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" // local-part
         + "@"
         + @"((([\w]+([-\w]*[\w]+)*\.)+[a-zA-Z]+)|" // domain
         + @"((([01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]).){3}[01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]))\z";

        [JsonIgnore]
        public static string PhonePattern = @"^(\+7|7|8)?[\s\-]?\(?[489][0-9]{2}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$";
        [JsonIgnore]
        public static string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6 ,15}$";
        string password = "";
        string login = "";
        string firstName = "";
        string middleName = "";
        string lastName = "";
        string phone = "";
        string email = "";
        string phoneError = "";
        string cityError = "";
        string secondPasswordError = "";
        string emailError = "";
        string firstNameError = "";
        string lastNameError = "";
        string passwordError = "";
        string secondPassword = "";
        Regex reg = new Regex(ComplexEmailPattern4);
        Regex reg1 = new Regex(PhonePattern);
        Regex reg2 = new Regex(PasswordPattern);
        Match match;

        [JsonProperty("userType")]
        public string UserType { get; set; } = "IndividualEntity";

        [JsonIgnore]
        public DateTime DateOfBirth { get; set; } = new DateTime(1999, 3, 1);

        [JsonIgnore]
        public string Token { get; set; }

        [JsonIgnore]
        public string FIO
        {
            get => $"{LastName} {FirstName} {MiddleName}";
        }

        [JsonIgnore]
        public string ShortFIO
        {
            get => (LastName?.Length > 0 ? LastName.Substring(0, 1) : "") + (FirstName?.Length > 0 ? FirstName.Substring(0, 1) : "");
        }
        private int _BirthYear = 1980;
        public string SalaryLevel { get; set; } = "None";
        public int BirthYear
        {
            get => _BirthYear;
            set
            {
                if (_BirthYear != value)
                {
                    _BirthYear = value;
                    OnPropertyChanged(nameof(BirthYear));
                }
            }
        }
        public bool HasPhoneError => !string.IsNullOrWhiteSpace(PhoneError);
        public bool HasNameError => !string.IsNullOrWhiteSpace(FirstNameError);
        public bool HasSecondPasswordError => !string.IsNullOrWhiteSpace(SecondPasswordError);

        public bool HasPasswordError => !string.IsNullOrWhiteSpace(PasswordError);

        [JsonProperty("password")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value != password)
                {
                    if (value is null)
                        value = "";
                    password = value;
                    if (password?.Length < 6 || password?.ToLower() == password)
                        PasswordError = "Пароль должен содержать от 6 до 15 символов, содержать хотя бы одну заглавную букву и одну цифру!";
                    if (string.IsNullOrWhiteSpace(password))
                        PasswordError = "Поле \"Пароль\" не может быть пустым!";
                    if (password?.Length > 6 && password?.ToLower() != password && !string.IsNullOrWhiteSpace(password))
                    {
                        PasswordError = "";
                    }
                    OnPropertyChanged("Password");
                }
            }
        }

        [JsonIgnore]
        public string SecondPassword
        {
            get
            {
                return secondPassword;
            }
            set
            {
                if (value != secondPassword)
                {
                    if (value is null)
                        value = "";
                    secondPassword = value;
                    if (password != secondPassword)
                        SecondPasswordError = "Введенные пароли не совпадают!";
                    if (string.IsNullOrWhiteSpace(secondPassword))
                        SecondPasswordError = "Поле \"Подтверждение пароля\" не может быть пустым!";
                    if (password == secondPassword && !string.IsNullOrWhiteSpace(secondPassword))
                        SecondPasswordError = "";
                    OnPropertyChanged("SecondPassword");
                }
            }
        }

        [JsonProperty("phone")]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (value != phone)
                {
                    if (value is null)
                        value = "";
                    phone = value;
                    match = reg1.Match(phone);
                    if (!match.Success)
                        PhoneError = "Пожалуйста, введите корректный номер телефона!";
                    if (string.IsNullOrWhiteSpace(phone))
                        PhoneError = "Поле \"Телефон\" не может быть пустым!";
                    if (match.Success && !string.IsNullOrWhiteSpace(phone))
                        PhoneError = "";
                    OnPropertyChanged("Phone");
                }
            }
        }

        [JsonIgnore]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                {
                    if (value is null)
                        value = "";
                    email = value;
                    match = reg.Match(email);
                    if (!match.Success)
                        EmailError = "Пожалуйста, введите корректный адрес электронной почты!";
                    if (string.IsNullOrWhiteSpace(email))
                        EmailError = "Поле \"Email\" не может быть пустым!";
                    if (match.Success && !string.IsNullOrWhiteSpace(email))
                        EmailError = "";
                    OnPropertyChanged("Email");
                }
            }
        }

        [JsonIgnore]
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (value != login)
                {
                    login = value;
                    OnPropertyChanged("Login");
                }
            }
        }

        [JsonProperty("Name")]
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value != firstName)
                {
                    if (value is null)
                        value = "";
                    firstName = value;
                    if (string.IsNullOrWhiteSpace(firstName))
                        FirstNameError = "Поле \"Имя\" не может быть пустым!";
                    else
                        FirstNameError = "";
                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("HasFirstName");
                }
            }
        }


        [JsonIgnore]
        public string MiddleName
        {
            get
            {
                return middleName;
            }
            set
            {
                if (value != middleName)
                {
                    if (value is null)
                        value = "";
                    middleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }


        [JsonIgnore]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value != lastName)
                {
                    if (value is null)
                        value = "";
                    lastName = value;
                    if (string.IsNullOrWhiteSpace(lastName))
                        LastNameError = "Поле \"Фамилия\" не может быть пустым!";
                    else
                        LastNameError = "";
                    OnPropertyChanged("LastName");
                }
            }
        }

        [JsonIgnore]
        public string SecondPasswordError
        {
            get => secondPasswordError;
            set
            {
                if (secondPasswordError != value)
                {
                    if (value is null)
                        value = "";
                    secondPasswordError = value;
                    OnPropertyChanged("SecondPasswordError");
                    OnPropertyChanged("HasSecondPasswordError");
                }
            }
        }

        [JsonIgnore]
        public string CityError
        {
            get => cityError;
            set
            {
                if (cityError != value)
                {
                    if (value is null)
                        value = "";
                    cityError = value;
                    OnPropertyChanged("CityError");
                }
            }
        }

        [JsonIgnore]
        public string PasswordError
        {
            get => passwordError;
            set
            {
                if (passwordError != value)
                {
                    if (value is null)
                        value = "";
                    passwordError = value;
                    OnPropertyChanged("PasswordError");
                    OnPropertyChanged("HasPasswordError");
                }
            }
        }

        [JsonIgnore]
        public string PhoneError
        {
            get => phoneError;
            set
            {
                if (phoneError != value)
                {
                    if (value is null)
                        value = "";
                    phoneError = value;
                    OnPropertyChanged("PhoneError");
                    OnPropertyChanged("HasPhoneError");
                }
            }
        }

        [JsonIgnore]
        public string EmailError
        {
            get => emailError;
            set
            {
                if (emailError != value)
                {
                    if (value is null)
                        value = "";
                    emailError = value;
                    OnPropertyChanged("EmailError");
                }
            }
        }

        [JsonIgnore]
        public string FirstNameError
        {
            get => firstNameError;
            set
            {
                if (firstNameError != value)
                {
                    if (value is null)
                        value = "";
                    firstNameError = value;
                    OnPropertyChanged("FirstNameError");
                    OnPropertyChanged("HasNameError");
                }
            }
        }

        [JsonIgnore]
        public string LastNameError
        {
            get => lastNameError;
            set
            {
                if (lastNameError != value)
                {
                    if (value is null)
                        value = "";
                    lastNameError = value;
                    OnPropertyChanged("LastNameError");
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string text = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(text));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
