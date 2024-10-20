namespace Common.Enums;

public enum UserStatus
{
    Empty = 0,
    DogrulamaBekliyor = 1,
    Aktif = 100,
    Pasif = 888,
    Silindi = 999
}
public static class UserStatusxt
{
    public static bool TryParse(int intValue, out UserStatus kullaniciDurum, out string hata)
    {
        hata = string.Empty;
        try
        {
            kullaniciDurum = (UserStatus)intValue;
            return true;
        }
        catch (Exception)
        {
            hata = "Kullanıcı durumu hatalı.";
            kullaniciDurum = UserStatus.Empty;
            return false;
        }
    }

    public static bool IsEmpty(int durum) => durum == 0;
    public static bool IsEmpty(this UserStatus durum) => (int)durum == 0;
    public static bool IsDogrulamaBekliyor(int durum) => durum == (int)UserStatus.DogrulamaBekliyor;
    public static bool IsDogrulamaBekliyor(this UserStatus durum) => durum == UserStatus.DogrulamaBekliyor;
    public static bool IsAktif(int durum) => durum == (int)UserStatus.Aktif;
    public static bool IsAktif(this UserStatus durum) => durum == UserStatus.Aktif;
    public static bool IsPasif(int durum) => durum == (int)UserStatus.Pasif;
    public static bool IsPasif(this UserStatus durum) => durum == UserStatus.Pasif;
    public static bool IsSilindi(int durum) => durum == (int)UserStatus.Silindi;
    public static bool IsSilindi(this UserStatus durum) => durum == UserStatus.Silindi;
}