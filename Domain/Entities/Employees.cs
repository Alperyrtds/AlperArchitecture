﻿using Domain.Primitives;

namespace Domain.Entities;

    public sealed class User : Entity
    {
        public User(Guid id, string name, string surname, DateTime birthDate, string identityNumber, string phoneNumber) : base(id)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            IdentityNumber = identityNumber;
            PhoneNumber = phoneNumber;
        }

        private User()
        {

        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string IdentityNumber { get; private set; }
        public string PhoneNumber { get; private set; }
    }

