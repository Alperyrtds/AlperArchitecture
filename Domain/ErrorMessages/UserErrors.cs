using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alper.Domain.ErrorMessages;

public static class UserErrors
{
    public const string IslemYapanKullaniciBosOlamaz = "İşlem yapan kullanıcı boş.";
    public const string KullaniciAktif = "Kullanıcı aktif.";
    public const string KullaniciPasif = "Kullanıcı pasif.";
    public const string KullaniciPasifDegil = "Kullanıcı pasif değil.";
    public const string KullaniciDogrulanmamis = "Kullanıcı doğrulanmamış.";
    public const string KullaniciDurumuBelirsiz = "Kullanıcı durumu belirsiz.";
    public const string KullaniciSilinmis = "Kullanıcı silinmiş.";
    public const string BilinmeyenKullaniciDurumu = "Kullanıcı durumu bilinmiyor.";

    public const string IslemYapanKullaniciBulunamadi = "İşlem yapan kullanıcı bulunamadı.";
    public const string IslemYapanKullaniciAktifDegil = "İşlem yapan kullanıcı aktif değil.";
    public const string IslemYapanKullaniciYetkiliDegil = "İşlem yapan kullanıcı yetkili değil.";
    public const string IslemYapanKullaniciDagitimSirketindeDegil = "İşlem yapan kullanıcı dağıtım şirketi kullanıcısı değil.";

    public const string KullaniciBulunamadi = "Kullanıcı bulunamadı.";
    public const string KullaniciListesiBulunamadi = "Kullanıcı listesi bulunamadı.";
    public const string KullaniciDurumuUygunDegil = "Kullanıcı durumu uygun değil.";
    public const string KullaniciKvkkOnayiZatenVar = "Kullanıcı kvkk onayı daha önce verilmiş.";
}
