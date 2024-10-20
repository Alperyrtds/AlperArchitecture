using NUlid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Utils;

public static class CheckIf
{
    public static bool ValidId(string id)
    {
        return Ulid.TryParse(id, out _);
    }

    public static bool ValidParola(string? parola)
    {
        if (string.IsNullOrWhiteSpace(parola))
        {
            return false;
        }

        var pattern = @"\s+";
        var whitespace = new Regex(pattern);
        var bosluksuz = whitespace.Replace(parola, string.Empty);

        if (bosluksuz != parola)
        {
            return false;
        }

        /*
           (?=.*?[A-Z]): Enaz bir büyük harf olmalı.

           (?=.*?[a-z]): En az bir küçük harf olmalı.

           (?=.*?[0-9]): En az bir rakam olmalı.

           (?=.*?[#?!@$%^&*-]): En az bir tane özel karakter olmalı

           {8,}: En az 8 karakter uzunluğunda olmalı
         */

        pattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
        var validateRegex = new Regex(pattern);
        return validateRegex.IsMatch(parola);
    }

    public static bool ValidIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var pattern = @"\s+";
        var whitespace = new Regex(pattern);
        var bosluksuz = whitespace.Replace(iban, string.Empty);

        if (bosluksuz != iban)
        {
            return false;
        }

        var numericIban = ConvertIbanToNumeric(iban);

        if (!Modulus97Check(numericIban))
        {
            return false;
        }

        pattern = @"TR[a-zA-Z0-9]{2}\s?([0-9]{4}\s?){1}([0-9]{1})([a-zA-Z0-9]{3}\s?)([a-zA-Z0-9]{4}\s?){3}([a-zA-Z0-9]{2})\s?";
        var validateRegex = new Regex(pattern);
        return validateRegex.IsMatch(iban);
    }

    private static string ConvertIbanToNumeric(string iban)
    {
        return iban[4..] + iban[..4]
            .Select(c => char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString())
            .Aggregate((a, b) => a + b);
    }

    private static bool Modulus97Check(string numericIban)
    {
        long remainder = 0;

        foreach (var digit in numericIban)
        {
            remainder = (remainder * 10 + (digit - '0')) % 97;
        }

        return remainder == 1;
    }


    public static bool ValidEMail(string? eMail)
    {
        return MailAddress.TryCreate(eMail, out _);
    }

    public static bool ValidKullaniciTelefonu(string? telefon)
    {
        if (string.IsNullOrWhiteSpace(telefon))
        {
            return true;
        }

        var pattern = @"\s+";
        var whitespace = new Regex(pattern);
        var tel = whitespace.Replace(telefon, string.Empty);

        if (tel[0] == '0')
        {
            return false;
        }

        if (tel.Length != 10)
        {
            return false;
        }

        pattern = "^(?=.*?[0-9])";
        var validateRegex = new Regex(pattern);
        return validateRegex.IsMatch(tel);
    }

    public static bool ValidEvetHayir(string islem)
    {
        if (islem.Length != 1)
        {
            return false;
        }

        return islem[0] == 'E' || islem[0] == 'H';
    }

    public static bool ValidDate(DateTime date)
    {
        return date != default;
    }
}
