using Luzyce.Api.Db.Subiekt.Models;
using Microsoft.EntityFrameworkCore;

namespace Luzyce.Api.Db.Subiekt.Data;

public partial class SubiektDbContext : DbContext
{
    public SubiektDbContext()
    {
    }

    public SubiektDbContext(DbContextOptions<SubiektDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdrHistorium> AdrHistoria { get; set; }

    public virtual DbSet<DokDokument> DokDokuments { get; set; }

    public virtual DbSet<DokPozycja> DokPozycjas { get; set; }

    public virtual DbSet<KhKontrahent> KhKontrahents { get; set; }

    public virtual DbSet<SlMagazyn> SlMagazyns { get; set; }

    public virtual DbSet<TwStan> TwStans { get; set; }

    public virtual DbSet<TwTowar> TwTowars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=SubiektConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdrHistorium>(entity =>
        {
            entity.HasKey(e => e.AdrhId);

            entity.ToTable("adr_Historia");

            entity.HasIndex(e => new { e.AdrhIdAdresu, e.AdrhId }, "IX_adr_Historia");

            entity.Property(e => e.AdrhId)
                .ValueGeneratedNever()
                .HasColumnName("adrh_Id");
            entity.Property(e => e.AdrhAdres)
                .HasMaxLength(82)
                .IsUnicode(false)
                .HasColumnName("adrh_Adres");
            entity.Property(e => e.AdrhDataZmiany)
                .HasColumnType("datetime")
                .HasColumnName("adrh_DataZmiany");
            entity.Property(e => e.AdrhFaks)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("adrh_Faks");
            entity.Property(e => e.AdrhGmina)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("adrh_Gmina");
            entity.Property(e => e.AdrhIdAdresu).HasColumnName("adrh_IdAdresu");
            entity.Property(e => e.AdrhIdGminy).HasColumnName("adrh_IdGminy");
            entity.Property(e => e.AdrhIdPanstwo).HasColumnName("adrh_IdPanstwo");
            entity.Property(e => e.AdrhIdWersja).HasColumnName("adrh_IdWersja");
            entity.Property(e => e.AdrhIdWojewodztwo).HasColumnName("adrh_IdWojewodztwo");
            entity.Property(e => e.AdrhIdZmienil).HasColumnName("adrh_IdZmienil");
            entity.Property(e => e.AdrhKod)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("adrh_Kod");
            entity.Property(e => e.AdrhMiejscowosc)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("adrh_Miejscowosc");
            entity.Property(e => e.AdrhNazwa)
                .HasMaxLength(53)
                .IsUnicode(false)
                .HasColumnName("adrh_Nazwa");
            entity.Property(e => e.AdrhNazwaPelna)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("adrh_NazwaPelna");
            entity.Property(e => e.AdrhNip)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("adrh_NIP");
            entity.Property(e => e.AdrhNrDomu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("adrh_NrDomu");
            entity.Property(e => e.AdrhNrEori)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("adrh_NrEORI");
            entity.Property(e => e.AdrhNrLokalu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("adrh_NrLokalu");
            entity.Property(e => e.AdrhNrUrzeduSkarbowego)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("adrh_NrUrzeduSkarbowego");
            entity.Property(e => e.AdrhPoczta)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("adrh_Poczta");
            entity.Property(e => e.AdrhPowiat)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("adrh_Powiat");
            entity.Property(e => e.AdrhSkrytka)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("adrh_Skrytka");
            entity.Property(e => e.AdrhSymbol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("adrh_Symbol");
            entity.Property(e => e.AdrhTelefon)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("adrh_Telefon");
            entity.Property(e => e.AdrhUlica)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("adrh_Ulica");
        });

        modelBuilder.Entity<DokDokument>(entity =>
        {
            entity.HasKey(e => e.DokId);

            entity.ToTable("dok__Dokument", tb =>
                {
                    tb.HasTrigger("tr_DokDokument_CheckUniqueNumerKSeF");
                    tb.HasTrigger("tr_DokDokument_CheckUniqueWegielNumerOswiadczenia");
                    tb.HasTrigger("tr_DokDokument_Deleting");
                    tb.HasTrigger("tr_DokDokument_Inserting");
                    tb.HasTrigger("tr_ZapisCzasu");
                });

            entity.HasIndex(e => new { e.DokDoDokId, e.DokTyp, e.DokPodtyp }, "IX_dok__Dokument");

            entity.HasIndex(e => e.DokDataWyst, "IX_dok__Dokument_1");

            entity.HasIndex(e => new { e.DokTyp, e.DokMagId, e.DokNr, e.DokNrRoz, e.DokDataWyst }, "IX_dok__Dokument_2");

            entity.HasIndex(e => e.DokDataOtrzym, "IX_dok__Dokument_3");

            entity.HasIndex(e => new { e.DokTyp, e.DokPodtyp, e.DokNrPelnyOryg, e.DokPlatnikId }, "IX_dok__Dokument_4");

            entity.HasIndex(e => new { e.DokId, e.DokNrPelny, e.DokDoDokNrPelny, e.DokOdbiorcaAdreshId }, "IX_dok__Dokument_5");

            entity.HasIndex(e => new { e.DokStatus, e.DokId }, "IX_dok__Dokument_6");

            entity.HasIndex(e => new { e.DokTyp, e.DokId }, "IX_dok__Dokument_7");

            entity.HasIndex(e => new { e.DokPlatnikId, e.DokId, e.DokOdbiorcaId }, "IX_dok__Dokument_8");

            entity.Property(e => e.DokId)
                .ValueGeneratedNever()
                .HasColumnName("dok_Id");
            entity.Property(e => e.DokAdresDostawyAdreshId).HasColumnName("dok_AdresDostawyAdreshId");
            entity.Property(e => e.DokAdresDostawyId).HasColumnName("dok_AdresDostawyId");
            entity.Property(e => e.DokAkcyzaZwolnienieId).HasColumnName("dok_AkcyzaZwolnienieId");
            entity.Property(e => e.DokAlgorytm).HasColumnName("dok_Algorytm");
            entity.Property(e => e.DokBladKseF)
                .IsUnicode(false)
                .HasColumnName("dok_BladKSeF");
            entity.Property(e => e.DokCenyDataKursu)
                .HasColumnType("datetime")
                .HasColumnName("dok_CenyDataKursu");
            entity.Property(e => e.DokCenyIdBanku).HasColumnName("dok_CenyIdBanku");
            entity.Property(e => e.DokCenyKurs)
                .HasColumnType("money")
                .HasColumnName("dok_CenyKurs");
            entity.Property(e => e.DokCenyLiczbaJednostek).HasColumnName("dok_CenyLiczbaJednostek");
            entity.Property(e => e.DokCenyNarzut)
                .HasColumnType("money")
                .HasColumnName("dok_CenyNarzut");
            entity.Property(e => e.DokCenyPoziom).HasColumnName("dok_CenyPoziom");
            entity.Property(e => e.DokCenyRodzajKursu).HasColumnName("dok_CenyRodzajKursu");
            entity.Property(e => e.DokCenyTyp).HasColumnName("dok_CenyTyp");
            entity.Property(e => e.DokCesjaPlatnikOdbiorca).HasColumnName("dok_CesjaPlatnikOdbiorca");
            entity.Property(e => e.DokCzasPrzewozuTransportu)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("dok_CzasPrzewozuTransportu");
            entity.Property(e => e.DokCzasWysylkiTransportu)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("dok_CzasWysylkiTransportu");
            entity.Property(e => e.DokCzekaNaKseF).HasColumnName("dok_CzekaNaKSeF");
            entity.Property(e => e.DokDataMag)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataMag");
            entity.Property(e => e.DokDataNumeruKseF)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataNumeruKSeF");
            entity.Property(e => e.DokDataNumeruKseForyg)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("dok_DataNumeruKSeFOryg");
            entity.Property(e => e.DokDataOtrzym)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataOtrzym");
            entity.Property(e => e.DokDataRozpoczeciaPrzetwarzaniaKseF)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataRozpoczeciaPrzetwarzaniaKSeF");
            entity.Property(e => e.DokDataUjeciaKorekty)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataUjeciaKorekty");
            entity.Property(e => e.DokDataWyst)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataWyst");
            entity.Property(e => e.DokDataWystawieniaKseF)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataWystawieniaKSeF");
            entity.Property(e => e.DokDataZakonczenia)
                .HasColumnType("datetime")
                .HasColumnName("dok_DataZakonczenia");
            entity.Property(e => e.DokDefiniowalnyId).HasColumnName("dok_DefiniowalnyId");
            entity.Property(e => e.DokDoDokDataWyst)
                .HasColumnType("datetime")
                .HasColumnName("dok_DoDokDataWyst");
            entity.Property(e => e.DokDoDokId).HasColumnName("dok_DoDokId");
            entity.Property(e => e.DokDoDokNrPelny)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("dok_DoDokNrPelny");
            entity.Property(e => e.DokDoNumerKseF)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("dok_DoNumerKSeF");
            entity.Property(e => e.DokDodatkoweInfoRodzajuTransportu)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("dok_DodatkoweInfoRodzajuTransportu");
            entity.Property(e => e.DokDokumentFiskalnyDlaPodatnikaVat).HasColumnName("dok_DokumentFiskalnyDlaPodatnikaVat");
            entity.Property(e => e.DokDstNr).HasColumnName("dok_DstNr");
            entity.Property(e => e.DokDstNrPelny)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("dok_DstNrPelny");
            entity.Property(e => e.DokDstNrRoz)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dok_DstNrRoz");
            entity.Property(e => e.DokFakturaUproszczona).HasColumnName("dok_FakturaUproszczona");
            entity.Property(e => e.DokFiskalizacjaData)
                .HasColumnType("datetime")
                .HasColumnName("dok_FiskalizacjaData");
            entity.Property(e => e.DokFiskalizacjaIdUrzadzenia)
                .HasMaxLength(40)
                .HasColumnName("dok_FiskalizacjaIdUrzadzenia");
            entity.Property(e => e.DokFiskalizacjaNumer)
                .HasMaxLength(60)
                .HasColumnName("dok_FiskalizacjaNumer");
            entity.Property(e => e.DokFormaDokumentowania).HasColumnName("dok_FormaDokumentowania");
            entity.Property(e => e.DokIdPanstwaKonsumenta).HasColumnName("dok_IdPanstwaKonsumenta");
            entity.Property(e => e.DokIdPanstwaRozpoczeciaWysylki).HasColumnName("dok_IdPanstwaRozpoczeciaWysylki");
            entity.Property(e => e.DokIdPrzetwarzaniaKseF)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("dok_IdPrzetwarzaniaKSeF");
            entity.Property(e => e.DokIdPrzyczynyZwolnieniaZvat).HasColumnName("dok_IdPrzyczynyZwolnieniaZVAT");
            entity.Property(e => e.DokIdSesjiKasowej).HasColumnName("dok_IdSesjiKasowej");
            entity.Property(e => e.DokInformacjeDodatkowe)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("dok_InformacjeDodatkowe");
            entity.Property(e => e.DokJestHop).HasColumnName("dok_JestHOP");
            entity.Property(e => e.DokJestRuchMag).HasColumnName("dok_JestRuchMag");
            entity.Property(e => e.DokJestTylkoDoOdczytu).HasColumnName("dok_JestTylkoDoOdczytu");
            entity.Property(e => e.DokJestVatAuto).HasColumnName("dok_JestVatAuto");
            entity.Property(e => e.DokJestVatNaEksport).HasColumnName("dok_JestVatNaEksport");
            entity.Property(e => e.DokJestZmianaDatyDokKas).HasColumnName("dok_JestZmianaDatyDokKas");
            entity.Property(e => e.DokKartaId).HasColumnName("dok_KartaId");
            entity.Property(e => e.DokKatId).HasColumnName("dok_KatId");
            entity.Property(e => e.DokKodRodzajuTransakcji).HasColumnName("dok_KodRodzajuTransakcji");
            entity.Property(e => e.DokKodRodzajuTransportu).HasColumnName("dok_KodRodzajuTransportu");
            entity.Property(e => e.DokKorektaDanychNabywcy).HasColumnName("dok_KorektaDanychNabywcy");
            entity.Property(e => e.DokKredytId).HasColumnName("dok_KredytId");
            entity.Property(e => e.DokKwDoZaplaty)
                .HasColumnType("money")
                .HasColumnName("dok_KwDoZaplaty");
            entity.Property(e => e.DokKwGotowka)
                .HasColumnType("money")
                .HasColumnName("dok_KwGotowka");
            entity.Property(e => e.DokKwGotowkaPrzedplata)
                .HasColumnType("money")
                .HasColumnName("dok_KwGotowkaPrzedplata");
            entity.Property(e => e.DokKwKarta)
                .HasColumnType("money")
                .HasColumnName("dok_KwKarta");
            entity.Property(e => e.DokKwKartaPrzedplata)
                .HasColumnType("money")
                .HasColumnName("dok_KwKartaPrzedplata");
            entity.Property(e => e.DokKwKredyt)
                .HasColumnType("money")
                .HasColumnName("dok_KwKredyt");
            entity.Property(e => e.DokKwPrzelew)
                .HasColumnType("money")
                .HasColumnName("dok_KwPrzelew");
            entity.Property(e => e.DokKwPrzelewPrzedplata)
                .HasColumnType("money")
                .HasColumnName("dok_KwPrzelewPrzedplata");
            entity.Property(e => e.DokKwReszta)
                .HasColumnType("money")
                .HasColumnName("dok_KwReszta");
            entity.Property(e => e.DokKwWartosc)
                .HasColumnType("money")
                .HasColumnName("dok_KwWartosc");
            entity.Property(e => e.DokMagId).HasColumnName("dok_MagId");
            entity.Property(e => e.DokMechanizmPodzielonejPlatnosci).HasColumnName("dok_MechanizmPodzielonejPlatnosci");
            entity.Property(e => e.DokMetodaKasowa).HasColumnName("dok_MetodaKasowa");
            entity.Property(e => e.DokMscWyst)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dok_MscWyst");
            entity.Property(e => e.DokNaliczajFundusze).HasColumnName("dok_NaliczajFundusze");
            entity.Property(e => e.DokNr).HasColumnName("dok_Nr");
            entity.Property(e => e.DokNrIdentNabywcy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dok_NrIdentNabywcy");
            entity.Property(e => e.DokNrPelny)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("dok_NrPelny");
            entity.Property(e => e.DokNrPelnyOryg)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("dok_NrPelnyOryg");
            entity.Property(e => e.DokNrRachunkuBankowegoPdm).HasColumnName("dok_NrRachunkuBankowegoPdm");
            entity.Property(e => e.DokNrRoz)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dok_NrRoz");
            entity.Property(e => e.DokNumerKseF)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("dok_NumerKSeF");
            entity.Property(e => e.DokObiektGt).HasColumnName("dok_ObiektGT");
            entity.Property(e => e.DokObslugaDokDost).HasColumnName("dok_ObslugaDokDost");
            entity.Property(e => e.DokOdbiorcaAdreshId).HasColumnName("dok_OdbiorcaAdreshId");
            entity.Property(e => e.DokOdbiorcaId).HasColumnName("dok_OdbiorcaId");
            entity.Property(e => e.DokOdebral)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dok_Odebral");
            entity.Property(e => e.DokPersonelId).HasColumnName("dok_PersonelId");
            entity.Property(e => e.DokPlatId).HasColumnName("dok_PlatId");
            entity.Property(e => e.DokPlatTermin)
                .HasColumnType("datetime")
                .HasColumnName("dok_PlatTermin");
            entity.Property(e => e.DokPlatnikAdreshId).HasColumnName("dok_PlatnikAdreshId");
            entity.Property(e => e.DokPlatnikId).HasColumnName("dok_PlatnikId");
            entity.Property(e => e.DokPodlegaOplSpec).HasColumnName("dok_PodlegaOplSpec");
            entity.Property(e => e.DokPodpisanoElektronicznie).HasColumnName("dok_PodpisanoElektronicznie");
            entity.Property(e => e.DokPodsumaVatFszk).HasColumnName("dok_PodsumaVatFSzk");
            entity.Property(e => e.DokPodtyp).HasColumnName("dok_Podtyp");
            entity.Property(e => e.DokPodtytul)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dok_Podtytul");
            entity.Property(e => e.DokProceduraMarzy).HasColumnName("dok_ProceduraMarzy");
            entity.Property(e => e.DokPromoZenCardStatus).HasColumnName("dok_PromoZenCardStatus");
            entity.Property(e => e.DokPrzetworzonoZkwZd).HasColumnName("dok_PrzetworzonoZKwZD");
            entity.Property(e => e.DokRabatProc)
                .HasColumnType("money")
                .HasColumnName("dok_RabatProc");
            entity.Property(e => e.DokRejId).HasColumnName("dok_RejId");
            entity.Property(e => e.DokRodzajOperacjiVat).HasColumnName("dok_RodzajOperacjiVat");
            entity.Property(e => e.DokRozliczony).HasColumnName("dok_Rozliczony");
            entity.Property(e => e.DokSelloData)
                .HasColumnType("datetime")
                .HasColumnName("dok_SelloData");
            entity.Property(e => e.DokSelloId).HasColumnName("dok_SelloId");
            entity.Property(e => e.DokSelloSymbol)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("dok_SelloSymbol");
            entity.Property(e => e.DokSesjaKseF)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("dok_SesjaKSeF");
            entity.Property(e => e.DokSesjaKseFid).HasColumnName("dok_SesjaKSeFId");
            entity.Property(e => e.DokSrodowiskoKseF).HasColumnName("dok_SrodowiskoKSeF");
            entity.Property(e => e.DokStatus).HasColumnName("dok_Status");
            entity.Property(e => e.DokStatusBlok).HasColumnName("dok_StatusBlok");
            entity.Property(e => e.DokStatusEx).HasColumnName("dok_StatusEx");
            entity.Property(e => e.DokStatusFiskal).HasColumnName("dok_StatusFiskal");
            entity.Property(e => e.DokStatusKseF).HasColumnName("dok_StatusKSeF");
            entity.Property(e => e.DokStatusKsieg).HasColumnName("dok_StatusKsieg");
            entity.Property(e => e.DokSzybkaPlatnosc).HasColumnName("dok_SzybkaPlatnosc");
            entity.Property(e => e.DokTermPlatIdKonfig).HasColumnName("dok_TermPlatIdKonfig");
            entity.Property(e => e.DokTermPlatIdZadania).HasColumnName("dok_TermPlatIdZadania");
            entity.Property(e => e.DokTermPlatStatus).HasColumnName("dok_TermPlatStatus");
            entity.Property(e => e.DokTermPlatTerminalId)
                .HasMaxLength(40)
                .HasColumnName("dok_TermPlatTerminalId");
            entity.Property(e => e.DokTermPlatTransId)
                .HasMaxLength(128)
                .HasColumnName("dok_TermPlatTransId");
            entity.Property(e => e.DokTerminRealizacji)
                .HasColumnType("datetime")
                .HasColumnName("dok_TerminRealizacji");
            entity.Property(e => e.DokTransakcjaData)
                .HasColumnType("datetime")
                .HasColumnName("dok_TransakcjaData");
            entity.Property(e => e.DokTransakcjaId).HasColumnName("dok_TransakcjaId");
            entity.Property(e => e.DokTransakcjaJednolitaId).HasColumnName("dok_TransakcjaJednolitaId");
            entity.Property(e => e.DokTransakcjaSymbol)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("dok_TransakcjaSymbol");
            entity.Property(e => e.DokTyp).HasColumnName("dok_Typ");
            entity.Property(e => e.DokTypDatyUjeciaKorekty).HasColumnName("dok_TypDatyUjeciaKorekty");
            entity.Property(e => e.DokTypNrIdentNabywcy).HasColumnName("dok_TypNrIdentNabywcy");
            entity.Property(e => e.DokTytul)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dok_Tytul");
            entity.Property(e => e.DokUwagi)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("dok_Uwagi");
            entity.Property(e => e.DokUwagiExt)
                .HasMaxLength(3500)
                .IsUnicode(false)
                .HasColumnName("dok_UwagiExt");
            entity.Property(e => e.DokVatMarza).HasColumnName("dok_VatMarza");
            entity.Property(e => e.DokVatRozliczanyPrzezUslugobiorce).HasColumnName("dok_VatRozliczanyPrzezUslugobiorce");
            entity.Property(e => e.DokVenderoData)
                .HasColumnType("datetime")
                .HasColumnName("dok_VenderoData");
            entity.Property(e => e.DokVenderoId).HasColumnName("dok_VenderoId");
            entity.Property(e => e.DokVenderoStatus).HasColumnName("dok_VenderoStatus");
            entity.Property(e => e.DokVenderoSymbol)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("dok_VenderoSymbol");
            entity.Property(e => e.DokWaluta)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dok_Waluta");
            entity.Property(e => e.DokWalutaDataKursu)
                .HasColumnType("datetime")
                .HasColumnName("dok_WalutaDataKursu");
            entity.Property(e => e.DokWalutaIdBanku).HasColumnName("dok_WalutaIdBanku");
            entity.Property(e => e.DokWalutaKurs)
                .HasColumnType("money")
                .HasColumnName("dok_WalutaKurs");
            entity.Property(e => e.DokWalutaLiczbaJednostek).HasColumnName("dok_WalutaLiczbaJednostek");
            entity.Property(e => e.DokWalutaRodzajKursu).HasColumnName("dok_WalutaRodzajKursu");
            entity.Property(e => e.DokWartBrutto)
                .HasColumnType("money")
                .HasColumnName("dok_WartBrutto");
            entity.Property(e => e.DokWartMag)
                .HasColumnType("money")
                .HasColumnName("dok_WartMag");
            entity.Property(e => e.DokWartMagP)
                .HasColumnType("money")
                .HasColumnName("dok_WartMagP");
            entity.Property(e => e.DokWartMagR)
                .HasColumnType("money")
                .HasColumnName("dok_WartMagR");
            entity.Property(e => e.DokWartNetto)
                .HasColumnType("money")
                .HasColumnName("dok_WartNetto");
            entity.Property(e => e.DokWartOpWyd)
                .HasColumnType("money")
                .HasColumnName("dok_WartOpWyd");
            entity.Property(e => e.DokWartOpZwr)
                .HasColumnType("money")
                .HasColumnName("dok_WartOpZwr");
            entity.Property(e => e.DokWartOplRecykl)
                .HasColumnType("money")
                .HasColumnName("dok_WartOplRecykl");
            entity.Property(e => e.DokWartTwBrutto)
                .HasColumnType("money")
                .HasColumnName("dok_WartTwBrutto");
            entity.Property(e => e.DokWartTwNetto)
                .HasColumnType("money")
                .HasColumnName("dok_WartTwNetto");
            entity.Property(e => e.DokWartUsBrutto)
                .HasColumnType("money")
                .HasColumnName("dok_WartUsBrutto");
            entity.Property(e => e.DokWartUsNetto)
                .HasColumnType("money")
                .HasColumnName("dok_WartUsNetto");
            entity.Property(e => e.DokWartVat)
                .HasColumnType("money")
                .HasColumnName("dok_WartVat");
            entity.Property(e => e.DokWegielNumerOswiadczenia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dok_WegielNumerOswiadczenia");
            entity.Property(e => e.DokWystawil)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dok_Wystawil");
            entity.Property(e => e.DokXmlHashKseF)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("dok_XmlHashKSeF");
            entity.Property(e => e.DokZaimportowanoDoEwidencjiAkcyzowej).HasColumnName("dok_ZaimportowanoDoEwidencjiAkcyzowej");
            entity.Property(e => e.DokZlecenieId).HasColumnName("dok_ZlecenieId");
            entity.Property(e => e.DokZnacznikiGtunaPozycji).HasColumnName("dok_ZnacznikiGTUNaPozycji");
        });

        modelBuilder.Entity<DokPozycja>(entity =>
        {
            entity.HasKey(e => e.ObId);

            entity.ToTable("dok_Pozycja");

            entity.HasIndex(e => e.ObDokHanId, "IX_dok_Pozycja");

            entity.HasIndex(e => e.ObDokMagId, "IX_dok_Pozycja_1");

            entity.HasIndex(e => e.ObDoId, "IX_dok_Pozycja_2");

            entity.HasIndex(e => e.ObTowId, "IX_dok_Pozycja_3");

            entity.HasIndex(e => new { e.ObId, e.ObDokHanId, e.ObDokMagId, e.ObIlosc, e.ObCenaNetto, e.ObCenaBrutto, e.ObWartNetto, e.ObWartBrutto }, "IX_dok_Pozycja_4");

            entity.HasIndex(e => new { e.ObId, e.ObDoId, e.ObZnak, e.ObStatus, e.ObDokHanId, e.ObDokMagId, e.ObTowId, e.ObIlosc, e.ObIloscMag, e.ObCenaWaluta, e.ObCenaNetto, e.ObCenaBrutto, e.ObVatProc }, "IX_dok_Pozycja_5");

            entity.HasIndex(e => new { e.ObTowId, e.ObDokHanId, e.ObId, e.ObDoId }, "IX_dok_Pozycja_6");

            entity.Property(e => e.ObId)
                .ValueGeneratedNever()
                .HasColumnName("ob_Id");
            entity.Property(e => e.ObAkcyza).HasColumnName("ob_Akcyza");
            entity.Property(e => e.ObAkcyzaKwota)
                .HasColumnType("money")
                .HasColumnName("ob_AkcyzaKwota");
            entity.Property(e => e.ObAkcyzaWartosc)
                .HasColumnType("money")
                .HasColumnName("ob_AkcyzaWartosc");
            entity.Property(e => e.ObCenaBrutto)
                .HasColumnType("money")
                .HasColumnName("ob_CenaBrutto");
            entity.Property(e => e.ObCenaMag)
                .HasColumnType("money")
                .HasColumnName("ob_CenaMag");
            entity.Property(e => e.ObCenaNabycia)
                .HasColumnType("money")
                .HasColumnName("ob_CenaNabycia");
            entity.Property(e => e.ObCenaNetto)
                .HasColumnType("money")
                .HasColumnName("ob_CenaNetto");
            entity.Property(e => e.ObCenaPobranaZcennika).HasColumnName("ob_CenaPobranaZCennika");
            entity.Property(e => e.ObCenaWaluta)
                .HasColumnType("money")
                .HasColumnName("ob_CenaWaluta");
            entity.Property(e => e.ObDoId).HasColumnName("ob_DoId");
            entity.Property(e => e.ObDokHanId).HasColumnName("ob_DokHanId");
            entity.Property(e => e.ObDokHanLp).HasColumnName("ob_DokHanLp");
            entity.Property(e => e.ObDokMagId).HasColumnName("ob_DokMagId");
            entity.Property(e => e.ObDokMagLp).HasColumnName("ob_DokMagLp");
            entity.Property(e => e.ObIlosc)
                .HasColumnType("money")
                .HasColumnName("ob_Ilosc");
            entity.Property(e => e.ObIloscMag)
                .HasColumnType("money")
                .HasColumnName("ob_IloscMag");
            entity.Property(e => e.ObJm)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ob_Jm");
            entity.Property(e => e.ObKategoriaId).HasColumnName("ob_KategoriaId");
            entity.Property(e => e.ObKsefUuid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ob_KsefUUID");
            entity.Property(e => e.ObMagId).HasColumnName("ob_MagId");
            entity.Property(e => e.ObNumerSeryjny)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("ob_NumerSeryjny");
            entity.Property(e => e.ObOpis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ob_Opis");
            entity.Property(e => e.ObOplCukrowaCukierZawartoscEx)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaCukierZawartoscEx");
            entity.Property(e => e.ObOplCukrowaKwCukier)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaKwCukier");
            entity.Property(e => e.ObOplCukrowaKwCukierEx)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaKwCukierEx");
            entity.Property(e => e.ObOplCukrowaKwKofeina)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaKwKofeina");
            entity.Property(e => e.ObOplCukrowaKwSuma)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaKwSuma");
            entity.Property(e => e.ObOplCukrowaObj)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaObj");
            entity.Property(e => e.ObOplCukrowaParametry).HasColumnName("ob_OplCukrowaParametry");
            entity.Property(e => e.ObOplCukrowaPodlega).HasColumnName("ob_OplCukrowaPodlega");
            entity.Property(e => e.ObOplCukrowaWartCukier)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaWartCukier");
            entity.Property(e => e.ObOplCukrowaWartKofeina)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaWartKofeina");
            entity.Property(e => e.ObOplCukrowaWartSuma)
                .HasColumnType("money")
                .HasColumnName("ob_OplCukrowaWartSuma");
            entity.Property(e => e.ObOznaczenieGtu).HasColumnName("ob_OznaczenieGTU");
            entity.Property(e => e.ObPrzyczynaKorektyId).HasColumnName("ob_PrzyczynaKorektyId");
            entity.Property(e => e.ObRabat)
                .HasColumnType("money")
                .HasColumnName("ob_Rabat");
            entity.Property(e => e.ObStatus).HasColumnName("ob_Status");
            entity.Property(e => e.ObSyncId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ob_SyncId");
            entity.Property(e => e.ObTermin)
                .HasColumnType("datetime")
                .HasColumnName("ob_Termin");
            entity.Property(e => e.ObTowId).HasColumnName("ob_TowId");
            entity.Property(e => e.ObTowKodCn)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ob_TowKodCN");
            entity.Property(e => e.ObTowPkwiu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ob_TowPkwiu");
            entity.Property(e => e.ObTowRodzaj).HasColumnName("ob_TowRodzaj");
            entity.Property(e => e.ObVatId).HasColumnName("ob_VatId");
            entity.Property(e => e.ObVatProc)
                .HasColumnType("money")
                .HasColumnName("ob_VatProc");
            entity.Property(e => e.ObWartBrutto)
                .HasColumnType("money")
                .HasColumnName("ob_WartBrutto");
            entity.Property(e => e.ObWartMag)
                .HasColumnType("money")
                .HasColumnName("ob_WartMag");
            entity.Property(e => e.ObWartNabycia)
                .HasColumnType("money")
                .HasColumnName("ob_WartNabycia");
            entity.Property(e => e.ObWartNetto)
                .HasColumnType("money")
                .HasColumnName("ob_WartNetto");
            entity.Property(e => e.ObWartVat)
                .HasColumnType("money")
                .HasColumnName("ob_WartVat");
            entity.Property(e => e.ObWegielDataWprowadzeniaLubNabycia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ob_WegielDataWprowadzeniaLubNabycia");
            entity.Property(e => e.ObWegielOpisPochodzenia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ob_WegielOpisPochodzenia");
            entity.Property(e => e.ObZnak).HasColumnName("ob_Znak");

            entity.HasOne(d => d.ObDo).WithMany(p => p.InverseObDo)
                .HasForeignKey(d => d.ObDoId)
                .HasConstraintName("FK_dok_Pozycja_dok_Pozycja");

            entity.HasOne(d => d.ObDokHan).WithMany(p => p.DokPozycjaObDokHans)
                .HasForeignKey(d => d.ObDokHanId)
                .HasConstraintName("FK_dok_Pozycja_dok__Dokument");

            entity.HasOne(d => d.ObDokMag).WithMany(p => p.DokPozycjaObDokMags)
                .HasForeignKey(d => d.ObDokMagId)
                .HasConstraintName("FK_dok_Pozycja_dok__Dokument1");

            entity.HasOne(d => d.ObTow).WithMany(p => p.DokPozycjas)
                .HasForeignKey(d => d.ObTowId)
                .HasConstraintName("FK_dok_Pozycja_tw__Towar");
        });

        modelBuilder.Entity<KhKontrahent>(entity =>
        {
            entity.HasKey(e => e.KhId);

            entity.ToTable("kh__Kontrahent", tb =>
                {
                    tb.HasTrigger("tr_KhKontrahent_Deleting");
                    tb.HasTrigger("tr_KhKontrahent_Inserting");
                    tb.HasTrigger("tr_KhKontrahent_Updating");
                });

            entity.Property(e => e.KhId)
                .ValueGeneratedNever()
                .HasColumnName("kh_Id");
            entity.Property(e => e.KhAdresDostawy).HasColumnName("kh_AdresDostawy");
            entity.Property(e => e.KhAdresKoresp).HasColumnName("kh_AdresKoresp");
            entity.Property(e => e.KhAkcyza).HasColumnName("kh_Akcyza");
            entity.Property(e => e.KhBrakPpdlaRozrachunkowAuto).HasColumnName("kh_BrakPPDlaRozrachunkowAuto");
            entity.Property(e => e.KhCelZakupu).HasColumnName("kh_CelZakupu");
            entity.Property(e => e.KhCena).HasColumnName("kh_Cena");
            entity.Property(e => e.KhCentrumAut).HasColumnName("kh_CentrumAut");
            entity.Property(e => e.KhCrm).HasColumnName("kh_CRM");
            entity.Property(e => e.KhCzyKomunikat).HasColumnName("kh_CzyKomunikat");
            entity.Property(e => e.KhCzynnyPodatnikVat).HasColumnName("kh_CzynnyPodatnikVAT");
            entity.Property(e => e.KhDataDodania)
                .HasColumnType("datetime")
                .HasColumnName("kh_DataDodania");
            entity.Property(e => e.KhDataOkolicznosciowa)
                .HasColumnType("datetime")
                .HasColumnName("kh_DataOkolicznosciowa");
            entity.Property(e => e.KhDataVat)
                .HasColumnType("datetime")
                .HasColumnName("kh_DataVAT");
            entity.Property(e => e.KhDataWaznosciVat)
                .HasColumnType("datetime")
                .HasColumnName("kh_DataWaznosciVAT");
            entity.Property(e => e.KhDataWyd)
                .HasColumnType("datetime")
                .HasColumnName("kh_DataWyd");
            entity.Property(e => e.KhDataZmiany)
                .HasColumnType("datetime")
                .HasColumnName("kh_DataZmiany");
            entity.Property(e => e.KhDomena)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Domena");
            entity.Property(e => e.KhDomyslnaFormaDokumentowaniaSprzedaz).HasColumnName("kh_DomyslnaFormaDokumentowaniaSprzedaz");
            entity.Property(e => e.KhDomyslnaTransVatsprzedaz).HasColumnName("kh_DomyslnaTransVATSprzedaz");
            entity.Property(e => e.KhDomyslnaTransVatsprzedazFw).HasColumnName("kh_DomyslnaTransVATSprzedazFW");
            entity.Property(e => e.KhDomyslnaTransVatzakup).HasColumnName("kh_DomyslnaTransVATZakup");
            entity.Property(e => e.KhDomyslnaTransVatzakupFw).HasColumnName("kh_DomyslnaTransVATZakupFW");
            entity.Property(e => e.KhDomyslnaWaluta)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("kh_DomyslnaWaluta");
            entity.Property(e => e.KhDomyslnaWalutaMode).HasColumnName("kh_DomyslnaWalutaMode");
            entity.Property(e => e.KhDomyslnyRachBankowyId).HasColumnName("kh_DomyslnyRachBankowyId");
            entity.Property(e => e.KhDomyslnyRachBankowyIdMode).HasColumnName("kh_DomyslnyRachBankowyIdMode");
            entity.Property(e => e.KhDomyslnyTypCeny).HasColumnName("kh_DomyslnyTypCeny");
            entity.Property(e => e.KhEfakturyData)
                .HasColumnType("datetime")
                .HasColumnName("kh_EFakturyData");
            entity.Property(e => e.KhEfakturyZgoda).HasColumnName("kh_EFakturyZgoda");
            entity.Property(e => e.KhEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_EMail");
            entity.Property(e => e.KhEwVatspMcOdliczenia).HasColumnName("kh_EwVATSpMcOdliczenia");
            entity.Property(e => e.KhEwVatzakMcOdliczenia).HasColumnName("kh_EwVATZakMcOdliczenia");
            entity.Property(e => e.KhEwVatzakRodzaj).HasColumnName("kh_EwVATZakRodzaj");
            entity.Property(e => e.KhEwVatzakSposobOdliczenia).HasColumnName("kh_EwVATZakSposobOdliczenia");
            entity.Property(e => e.KhGaduGadu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kh_GaduGadu");
            entity.Property(e => e.KhGrafika)
                .HasColumnType("image")
                .HasColumnName("kh_Grafika");
            entity.Property(e => e.KhIdBranza).HasColumnName("kh_IdBranza");
            entity.Property(e => e.KhIdDodal).HasColumnName("kh_IdDodal");
            entity.Property(e => e.KhIdEwVatsp).HasColumnName("kh_IdEwVATSp");
            entity.Property(e => e.KhIdEwVatspKateg).HasColumnName("kh_IdEwVATSpKateg");
            entity.Property(e => e.KhIdEwVatzak).HasColumnName("kh_IdEwVATZak");
            entity.Property(e => e.KhIdEwVatzakKateg).HasColumnName("kh_IdEwVATZakKateg");
            entity.Property(e => e.KhIdFormaP).HasColumnName("kh_IdFormaP");
            entity.Property(e => e.KhIdGrupa).HasColumnName("kh_IdGrupa");
            entity.Property(e => e.KhIdKategoriaKh).HasColumnName("kh_IdKategoriaKH");
            entity.Property(e => e.KhIdNabywca).HasColumnName("kh_IdNabywca");
            entity.Property(e => e.KhIdOdbiorca).HasColumnName("kh_IdOdbiorca");
            entity.Property(e => e.KhIdOpiekun).HasColumnName("kh_IdOpiekun");
            entity.Property(e => e.KhIdOsobaDo).HasColumnName("kh_IdOsobaDO");
            entity.Property(e => e.KhIdOstatniWpisWeryfikacjiStatusuVat).HasColumnName("kh_IdOstatniWpisWeryfikacjiStatusuVAT");
            entity.Property(e => e.KhIdOstatniWpisWeryfikacjiStatusuVies).HasColumnName("kh_IdOstatniWpisWeryfikacjiStatusuVIES");
            entity.Property(e => e.KhIdOstatniWpisWeryfikacjiWykazPodatnikowVat).HasColumnName("kh_IdOstatniWpisWeryfikacjiWykazPodatnikowVAT");
            entity.Property(e => e.KhIdPozyskany).HasColumnName("kh_IdPozyskany");
            entity.Property(e => e.KhIdRabat).HasColumnName("kh_IdRabat");
            entity.Property(e => e.KhIdRachKategPrzychod).HasColumnName("kh_IdRachKategPrzychod");
            entity.Property(e => e.KhIdRachKategRozchod).HasColumnName("kh_IdRachKategRozchod");
            entity.Property(e => e.KhIdRachunkuWirtualnego).HasColumnName("kh_IdRachunkuWirtualnego");
            entity.Property(e => e.KhIdRegion).HasColumnName("kh_IdRegion");
            entity.Property(e => e.KhIdRodzajKontaktu).HasColumnName("kh_IdRodzajKontaktu");
            entity.Property(e => e.KhIdZmienil).HasColumnName("kh_IdZmienil");
            entity.Property(e => e.KhImie)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kh_Imie");
            entity.Property(e => e.KhInstKredytowa).HasColumnName("kh_InstKredytowa");
            entity.Property(e => e.KhJednorazowy).HasColumnName("kh_Jednorazowy");
            entity.Property(e => e.KhKlientSklepuInternetowego).HasColumnName("kh_KlientSklepuInternetowego");
            entity.Property(e => e.KhKomunikat)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_Komunikat");
            entity.Property(e => e.KhKomunikatOd)
                .HasColumnType("datetime")
                .HasColumnName("kh_KomunikatOd");
            entity.Property(e => e.KhKomunikatZawsze).HasColumnName("kh_KomunikatZawsze");
            entity.Property(e => e.KhKontakt)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("kh_Kontakt");
            entity.Property(e => e.KhKorygowanieKup).HasColumnName("kh_KorygowanieKUP");
            entity.Property(e => e.KhKorygowanieVatsp).HasColumnName("kh_KorygowanieVATSp");
            entity.Property(e => e.KhKorygowanieVatzak).HasColumnName("kh_KorygowanieVATZak");
            entity.Property(e => e.KhKrs)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_KRS");
            entity.Property(e => e.KhKsefDodatkoweInfoPoprzedzEtykietaPola).HasColumnName("kh_KsefDodatkoweInfoPoprzedzEtykietaPola");
            entity.Property(e => e.KhKsefDomyslnyEtapPrzetwarzania).HasColumnName("kh_KsefDomyslnyEtapPrzetwarzania");
            entity.Property(e => e.KhKsefEksportObslugaPolaDodatkoweInformacje).HasColumnName("kh_KsefEksportObslugaPolaDodatkoweInformacje");
            entity.Property(e => e.KhKsefImportObslugaPolaDodatkoweInformacje).HasColumnName("kh_KsefImportObslugaPolaDodatkoweInformacje");
            entity.Property(e => e.KhKsefMagazynDlaEfaktur).HasColumnName("kh_KsefMagazynDlaEFaktur");
            entity.Property(e => e.KhKsefPoleDodatkoweInformacjeEksportNaPodstSql).HasColumnName("kh_KsefPoleDodatkoweInformacjeEksportNaPodstSql");
            entity.Property(e => e.KhKsefPoleDodatkoweInformacjeEksportSql)
                .HasColumnType("text")
                .HasColumnName("kh_KsefPoleDodatkoweInformacjeEksportSql");
            entity.Property(e => e.KhLiczbaPrac).HasColumnName("kh_LiczbaPrac");
            entity.Property(e => e.KhLokalizacja)
                .HasMaxLength(256)
                .HasColumnName("kh_Lokalizacja");
            entity.Property(e => e.KhMalyPojazd).HasColumnName("kh_MalyPojazd");
            entity.Property(e => e.KhMaxDniSp).HasColumnName("kh_MaxDniSp");
            entity.Property(e => e.KhMaxDokKred).HasColumnName("kh_MaxDokKred");
            entity.Property(e => e.KhMaxWartDokKred)
                .HasColumnType("money")
                .HasColumnName("kh_MaxWartDokKred");
            entity.Property(e => e.KhMaxWartKred)
                .HasColumnType("money")
                .HasColumnName("kh_MaxWartKred");
            entity.Property(e => e.KhMetodaKasowa).HasColumnName("kh_MetodaKasowa");
            entity.Property(e => e.KhNaliczajOplSpec).HasColumnName("kh_NaliczajOplSpec");
            entity.Property(e => e.KhNazwisko)
                .HasMaxLength(51)
                .IsUnicode(false)
                .HasColumnName("kh_Nazwisko");
            entity.Property(e => e.KhNrAkcyzowy)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("kh_NrAkcyzowy");
            entity.Property(e => e.KhNrAnalitykaD)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("kh_NrAnalitykaD");
            entity.Property(e => e.KhNrAnalitykaO)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("kh_NrAnalitykaO");
            entity.Property(e => e.KhNrDowodu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kh_NrDowodu");
            entity.Property(e => e.KhOdbDet).HasColumnName("kh_OdbDet");
            entity.Property(e => e.KhOdbiorcaCesjaPlatnosci).HasColumnName("kh_OdbiorcaCesjaPlatnosci");
            entity.Property(e => e.KhOpisOperacji)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_OpisOperacji");
            entity.Property(e => e.KhOrganWyd)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_OrganWyd");
            entity.Property(e => e.KhOsoba).HasColumnName("kh_Osoba");
            entity.Property(e => e.KhOsobaVat).HasColumnName("kh_OsobaVAT");
            entity.Property(e => e.KhOstrzezenieTerminPlatnosciPrzekroczony).HasColumnName("kh_OstrzezenieTerminPlatnosciPrzekroczony");
            entity.Property(e => e.KhPesel)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("kh_PESEL");
            entity.Property(e => e.KhPlatOdroczone).HasColumnName("kh_PlatOdroczone");
            entity.Property(e => e.KhPlatPrzelew).HasColumnName("kh_PlatPrzelew");
            entity.Property(e => e.KhPodVatzarejestrowanyWue).HasColumnName("kh_PodVATZarejestrowanyWUE");
            entity.Property(e => e.KhPodatekCukrowyNaliczaj).HasColumnName("kh_PodatekCukrowyNaliczaj");
            entity.Property(e => e.KhPole1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole1");
            entity.Property(e => e.KhPole2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole2");
            entity.Property(e => e.KhPole3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole3");
            entity.Property(e => e.KhPole4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole4");
            entity.Property(e => e.KhPole5)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole5");
            entity.Property(e => e.KhPole6)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole6");
            entity.Property(e => e.KhPole7)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole7");
            entity.Property(e => e.KhPole8)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_Pole8");
            entity.Property(e => e.KhPotencjalny).HasColumnName("kh_Potencjalny");
            entity.Property(e => e.KhPowitanie)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_Powitanie");
            entity.Property(e => e.KhPracownik)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("kh_Pracownik");
            entity.Property(e => e.KhPrefKontakt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kh_PrefKontakt");
            entity.Property(e => e.KhProcGotowka)
                .HasColumnType("money")
                .HasColumnName("kh_ProcGotowka");
            entity.Property(e => e.KhProcKarta)
                .HasColumnType("money")
                .HasColumnName("kh_ProcKarta");
            entity.Property(e => e.KhProcKredyt)
                .HasColumnType("money")
                .HasColumnName("kh_ProcKredyt");
            entity.Property(e => e.KhProcPozostalo)
                .HasColumnType("money")
                .HasColumnName("kh_ProcPozostalo");
            entity.Property(e => e.KhProcPrzelew)
                .HasColumnType("money")
                .HasColumnName("kh_ProcPrzelew");
            entity.Property(e => e.KhProducentRolny).HasColumnName("kh_ProducentRolny");
            entity.Property(e => e.KhPrzypadekSzczegolnyPit).HasColumnName("kh_PrzypadekSzczegolnyPIT");
            entity.Property(e => e.KhRegon)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kh_REGON");
            entity.Property(e => e.KhRodzaj).HasColumnName("kh_Rodzaj");
            entity.Property(e => e.KhSkype)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_Skype");
            entity.Property(e => e.KhStatusAkcyza).HasColumnName("kh_StatusAkcyza");
            entity.Property(e => e.KhStawkaVatprzychod).HasColumnName("kh_StawkaVATPrzychod");
            entity.Property(e => e.KhStawkaVatwydatek).HasColumnName("kh_StawkaVATWydatek");
            entity.Property(e => e.KhStosujIndywidualnyCennikWsklepieInternetowym).HasColumnName("kh_StosujIndywidualnyCennikWSklepieInternetowym");
            entity.Property(e => e.KhStosujRabatWmultistore).HasColumnName("kh_StosujRabatWMultistore");
            entity.Property(e => e.KhStosujSzybkaPlatnosc).HasColumnName("kh_StosujSzybkaPlatnosc");
            entity.Property(e => e.KhSymbol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kh_Symbol");
            entity.Property(e => e.KhTransakcjaVatsp).HasColumnName("kh_TransakcjaVATSp");
            entity.Property(e => e.KhTransakcjaVatzak).HasColumnName("kh_TransakcjaVATZak");
            entity.Property(e => e.KhUpowaznienieVat).HasColumnName("kh_UpowaznienieVAT");
            entity.Property(e => e.KhUwagi)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_Uwagi");
            entity.Property(e => e.KhVatRozliczanyPrzezUslugobiorce).HasColumnName("kh_VatRozliczanyPrzezUslugobiorce");
            entity.Property(e => e.KhVatRozliczanyPrzezUslugobiorceFw).HasColumnName("kh_VatRozliczanyPrzezUslugobiorceFW");
            entity.Property(e => e.KhWartoscNettoCzyBrutto).HasColumnName("kh_WartoscNettoCzyBrutto");
            entity.Property(e => e.KhWww)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("kh_WWW");
            entity.Property(e => e.KhWzwIdCrmTransakcja).HasColumnName("kh_WzwIdCrmTransakcja");
            entity.Property(e => e.KhWzwIdFs).HasColumnName("kh_WzwIdFS");
            entity.Property(e => e.KhWzwIdKfs).HasColumnName("kh_WzwIdKFS");
            entity.Property(e => e.KhWzwIdWz).HasColumnName("kh_WzwIdWZ");
            entity.Property(e => e.KhWzwIdWzvat).HasColumnName("kh_WzwIdWZVAT");
            entity.Property(e => e.KhWzwIdZd).HasColumnName("kh_WzwIdZD");
            entity.Property(e => e.KhWzwIdZk).HasColumnName("kh_WzwIdZK");
            entity.Property(e => e.KhWzwIdZkzal).HasColumnName("kh_WzwIdZKZAL");
            entity.Property(e => e.KhZablokowany).HasColumnName("kh_Zablokowany");
            entity.Property(e => e.KhZgodaDo).HasColumnName("kh_ZgodaDO");
            entity.Property(e => e.KhZgodaEmail).HasColumnName("kh_ZgodaEMail");
            entity.Property(e => e.KhZgodaMark).HasColumnName("kh_ZgodaMark");
            entity.Property(e => e.KhZgodaNewsletterVendero).HasColumnName("kh_ZgodaNewsletterVendero");

            entity.HasOne(d => d.KhIdNabywcaNavigation).WithMany(p => p.InverseKhIdNabywcaNavigation)
                .HasForeignKey(d => d.KhIdNabywca)
                .HasConstraintName("FK_kh__Kontrahent_kh__Kontrahent_Nabywca");

            entity.HasOne(d => d.KhIdOdbiorcaNavigation).WithMany(p => p.InverseKhIdOdbiorcaNavigation)
                .HasForeignKey(d => d.KhIdOdbiorca)
                .HasConstraintName("FK_kh__Kontrahent_kh__Kontrahent");
        });

        modelBuilder.Entity<SlMagazyn>(entity =>
        {
            entity.HasKey(e => e.MagId);

            entity.ToTable("sl_Magazyn", tb =>
                {
                    tb.HasTrigger("tr_MagazynInsMod");
                    tb.HasTrigger("tr_slMagazynInserting");
                    tb.HasTrigger("tr_sl_Magazyn_Deleting");
                });

            entity.Property(e => e.MagId)
                .ValueGeneratedNever()
                .HasColumnName("mag_Id");
            entity.Property(e => e.MagAnalityka)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("mag_Analityka");
            entity.Property(e => e.MagGlowny).HasColumnName("mag_Glowny");
            entity.Property(e => e.MagNazwa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mag_Nazwa");
            entity.Property(e => e.MagOpis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mag_Opis");
            entity.Property(e => e.MagPos).HasColumnName("mag_POS");
            entity.Property(e => e.MagPosadres)
                .HasMaxLength(82)
                .IsUnicode(false)
                .HasColumnName("mag_POSAdres");
            entity.Property(e => e.MagPosident).HasColumnName("mag_POSIdent");
            entity.Property(e => e.MagPosnazwa)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mag_POSNazwa");
            entity.Property(e => e.MagStatus).HasColumnName("mag_Status");
            entity.Property(e => e.MagSymbol)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("mag_Symbol");
        });

        modelBuilder.Entity<TwStan>(entity =>
        {
            entity.HasKey(e => new { e.StTowId, e.StMagId });

            entity.ToTable("tw_Stan");

            entity.HasIndex(e => new { e.StTowId, e.StMagId }, "IX_tw_Stany").IsUnique();

            entity.HasIndex(e => new { e.StTowId, e.StMagId, e.StStan, e.StStanRez }, "IX_tw_Stany_1");

            entity.Property(e => e.StTowId).HasColumnName("st_TowId");
            entity.Property(e => e.StMagId).HasColumnName("st_MagId");
            entity.Property(e => e.StStan)
                .HasColumnType("money")
                .HasColumnName("st_Stan");
            entity.Property(e => e.StStanMax)
                .HasColumnType("money")
                .HasColumnName("st_StanMax");
            entity.Property(e => e.StStanMin)
                .HasColumnType("money")
                .HasColumnName("st_StanMin");
            entity.Property(e => e.StStanRez)
                .HasColumnType("money")
                .HasColumnName("st_StanRez");

            entity.HasOne(d => d.StMag).WithMany(p => p.TwStans)
                .HasForeignKey(d => d.StMagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tw_Stan_sl_Magazyn");

            entity.HasOne(d => d.StTow).WithMany(p => p.TwStans)
                .HasForeignKey(d => d.StTowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tw_Stan_tw__Towar");
        });

        modelBuilder.Entity<TwTowar>(entity =>
        {
            entity.HasKey(e => e.TwId);

            entity.ToTable("tw__Towar", tb =>
                {
                    tb.HasTrigger("TRI_InsSearch_tw__Towar");
                    tb.HasTrigger("TRU_InsSearch_tw__Towar");
                    tb.HasTrigger("tr_TwTowar_Deleting");
                    tb.HasTrigger("tr_TwTowar_Inserting");
                    tb.HasTrigger("tr_TwTowar_Updating");
                    tb.HasTrigger("tr_tw__Towar_Synch_delete");
                    tb.HasTrigger("tr_tw__Towar_Synch_insert");
                    tb.HasTrigger("tr_tw__Towar_Synch_update");
                });

            entity.Property(e => e.TwId)
                .ValueGeneratedNever()
                .HasColumnName("tw_Id");
            entity.Property(e => e.TwAkcyza).HasColumnName("tw_Akcyza");
            entity.Property(e => e.TwAkcyzaKwota)
                .HasColumnType("money")
                .HasColumnName("tw_AkcyzaKwota");
            entity.Property(e => e.TwAkcyzaMarkaWyrobow)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("tw_AkcyzaMarkaWyrobow");
            entity.Property(e => e.TwAkcyzaWielkoscProducenta)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("tw_AkcyzaWielkoscProducenta");
            entity.Property(e => e.TwAkcyzaZaznacz).HasColumnName("tw_AkcyzaZaznacz");
            entity.Property(e => e.TwBloz12)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_bloz_12");
            entity.Property(e => e.TwBloz7)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_bloz_7");
            entity.Property(e => e.TwCenaOtwarta).HasColumnName("tw_CenaOtwarta");
            entity.Property(e => e.TwCharakter)
                .HasColumnType("text")
                .HasColumnName("tw_Charakter");
            entity.Property(e => e.TwCzasDostawy).HasColumnName("tw_CzasDostawy");
            entity.Property(e => e.TwDataZmianyVatSprzedazy)
                .HasColumnType("datetime")
                .HasColumnName("tw_DataZmianyVatSprzedazy");
            entity.Property(e => e.TwDniWaznosc).HasColumnName("tw_DniWaznosc");
            entity.Property(e => e.TwDodawalnyDoZw).HasColumnName("tw_DodawalnyDoZW");
            entity.Property(e => e.TwDomyslnaKategoria).HasColumnName("tw_DomyslnaKategoria");
            entity.Property(e => e.TwDostSymbol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tw_DostSymbol");
            entity.Property(e => e.TwGlebokosc)
                .HasColumnType("money")
                .HasColumnName("tw_Glebokosc");
            entity.Property(e => e.TwGrupaJpkVat).HasColumnName("tw_GrupaJpkVat");
            entity.Property(e => e.TwIdFundPromocji).HasColumnName("tw_IdFundPromocji");
            entity.Property(e => e.TwIdGrupa).HasColumnName("tw_IdGrupa");
            entity.Property(e => e.TwIdKoduWyrobuAkcyzowego).HasColumnName("tw_IdKoduWyrobuAkcyzowego");
            entity.Property(e => e.TwIdKrajuPochodzenia).HasColumnName("tw_IdKrajuPochodzenia");
            entity.Property(e => e.TwIdOpakowanie).HasColumnName("tw_IdOpakowanie");
            entity.Property(e => e.TwIdPodstDostawca).HasColumnName("tw_IdPodstDostawca");
            entity.Property(e => e.TwIdProducenta).HasColumnName("tw_IdProducenta");
            entity.Property(e => e.TwIdRabat).HasColumnName("tw_IdRabat");
            entity.Property(e => e.TwIdTypKodu).HasColumnName("tw_IdTypKodu");
            entity.Property(e => e.TwIdUjm).HasColumnName("tw_IdUJM");
            entity.Property(e => e.TwIdVatSp).HasColumnName("tw_IdVatSp");
            entity.Property(e => e.TwIdVatZak).HasColumnName("tw_IdVatZak");
            entity.Property(e => e.TwIsFundPromocji).HasColumnName("tw_IsFundPromocji");
            entity.Property(e => e.TwIsbn)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_isbn");
            entity.Property(e => e.TwJakPrzySp).HasColumnName("tw_JakPrzySp");
            entity.Property(e => e.TwJednMiary)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tw_JednMiary");
            entity.Property(e => e.TwJednMiarySprz)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tw_JednMiarySprz");
            entity.Property(e => e.TwJednMiaryZak)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tw_JednMiaryZak");
            entity.Property(e => e.TwJednStanMin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tw_JednStanMin");
            entity.Property(e => e.TwJmsprzInna).HasColumnName("tw_JMSprzInna");
            entity.Property(e => e.TwJmzakInna).HasColumnName("tw_JMZakInna");
            entity.Property(e => e.TwKodTowaru)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tw_KodTowaru");
            entity.Property(e => e.TwKodUproducenta)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_KodUProducenta");
            entity.Property(e => e.TwKomunikat)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_Komunikat");
            entity.Property(e => e.TwKomunikatDokumenty).HasColumnName("tw_KomunikatDokumenty");
            entity.Property(e => e.TwKomunikatOd)
                .HasColumnType("datetime")
                .HasColumnName("tw_KomunikatOd");
            entity.Property(e => e.TwKontrolaTw).HasColumnName("tw_KontrolaTW");
            entity.Property(e => e.TwLogo)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("tw_Logo");
            entity.Property(e => e.TwMasa)
                .HasColumnType("money")
                .HasColumnName("tw_Masa");
            entity.Property(e => e.TwMasaNetto)
                .HasColumnType("money")
                .HasColumnName("tw_MasaNetto");
            entity.Property(e => e.TwMechanizmPodzielonejPlatnosci).HasColumnName("tw_MechanizmPodzielonejPlatnosci");
            entity.Property(e => e.TwNazwa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Nazwa");
            entity.Property(e => e.TwObjetosc)
                .HasColumnType("money")
                .HasColumnName("tw_Objetosc");
            entity.Property(e => e.TwObrotMarza).HasColumnName("tw_ObrotMarza");
            entity.Property(e => e.TwOdwrotneObciazenie).HasColumnName("tw_OdwrotneObciazenie");
            entity.Property(e => e.TwOpis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_Opis");
            entity.Property(e => e.TwOplCukrowaInneSlodzace).HasColumnName("tw_OplCukrowaInneSlodzace");
            entity.Property(e => e.TwOplCukrowaKofeinaKwota)
                .HasColumnType("money")
                .HasColumnName("tw_OplCukrowaKofeinaKwota");
            entity.Property(e => e.TwOplCukrowaKofeinaPodlega).HasColumnName("tw_OplCukrowaKofeinaPodlega");
            entity.Property(e => e.TwOplCukrowaKwota)
                .HasColumnType("money")
                .HasColumnName("tw_OplCukrowaKwota");
            entity.Property(e => e.TwOplCukrowaKwotaPowyzej)
                .HasColumnType("money")
                .HasColumnName("tw_OplCukrowaKwotaPowyzej");
            entity.Property(e => e.TwOplCukrowaNapojWeglElektr).HasColumnName("tw_OplCukrowaNapojWeglElektr");
            entity.Property(e => e.TwOplCukrowaObj)
                .HasColumnType("money")
                .HasColumnName("tw_OplCukrowaObj");
            entity.Property(e => e.TwOplCukrowaPodlega).HasColumnName("tw_OplCukrowaPodlega");
            entity.Property(e => e.TwOplCukrowaSok).HasColumnName("tw_OplCukrowaSok");
            entity.Property(e => e.TwOplCukrowaZawartoscCukru)
                .HasColumnType("money")
                .HasColumnName("tw_OplCukrowaZawartoscCukru");
            entity.Property(e => e.TwPkwiU)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tw_PKWiU");
            entity.Property(e => e.TwPlu).HasColumnName("tw_PLU");
            entity.Property(e => e.TwPodlegaOplacieNaFunduszOchronyRolnictwa).HasColumnName("tw_PodlegaOplacieNaFunduszOchronyRolnictwa");
            entity.Property(e => e.TwPodstKodKresk)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tw_PodstKodKresk");
            entity.Property(e => e.TwPole1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole1");
            entity.Property(e => e.TwPole2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole2");
            entity.Property(e => e.TwPole3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole3");
            entity.Property(e => e.TwPole4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole4");
            entity.Property(e => e.TwPole5)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole5");
            entity.Property(e => e.TwPole6)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole6");
            entity.Property(e => e.TwPole7)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole7");
            entity.Property(e => e.TwPole8)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_Pole8");
            entity.Property(e => e.TwProgKwotowyOo).HasColumnName("tw_ProgKwotowyOO");
            entity.Property(e => e.TwPrzezWartosc).HasColumnName("tw_PrzezWartosc");
            entity.Property(e => e.TwRodzaj).HasColumnName("tw_Rodzaj");
            entity.Property(e => e.TwSerwisAukcyjny).HasColumnName("tw_SerwisAukcyjny");
            entity.Property(e => e.TwSklepInternet).HasColumnName("tw_SklepInternet");
            entity.Property(e => e.TwSprzedazMobilna).HasColumnName("tw_SprzedazMobilna");
            entity.Property(e => e.TwStanMaks)
                .HasColumnType("money")
                .HasColumnName("tw_StanMaks");
            entity.Property(e => e.TwStanMin)
                .HasColumnType("money")
                .HasColumnName("tw_StanMin");
            entity.Property(e => e.TwSww)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tw_SWW");
            entity.Property(e => e.TwSymbol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tw_Symbol");
            entity.Property(e => e.TwSzerokosc)
                .HasColumnType("money")
                .HasColumnName("tw_Szerokosc");
            entity.Property(e => e.TwUrzNazwa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tw_UrzNazwa");
            entity.Property(e => e.TwUsuniety).HasColumnName("tw_Usuniety");
            entity.Property(e => e.TwUwagi)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_Uwagi");
            entity.Property(e => e.TwWagaEtykiet).HasColumnName("tw_WagaEtykiet");
            entity.Property(e => e.TwWegielOpisPochodzenia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_WegielOpisPochodzenia");
            entity.Property(e => e.TwWegielPodlegaOswiadczeniu).HasColumnName("tw_WegielPodlegaOswiadczeniu");
            entity.Property(e => e.TwWww)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tw_WWW");
            entity.Property(e => e.TwWysokosc)
                .HasColumnType("money")
                .HasColumnName("tw_Wysokosc");
            entity.Property(e => e.TwZablokowany).HasColumnName("tw_Zablokowany");
            entity.Property(e => e.TwZnakiAkcyzy).HasColumnName("tw_ZnakiAkcyzy");

            entity.HasOne(d => d.TwIdOpakowanieNavigation).WithMany(p => p.InverseTwIdOpakowanieNavigation)
                .HasForeignKey(d => d.TwIdOpakowanie)
                .HasConstraintName("FK_tw__Towar_tw__Towar");

            entity.HasOne(d => d.TwIdPodstDostawcaNavigation).WithMany(p => p.TwTowarTwIdPodstDostawcaNavigations)
                .HasForeignKey(d => d.TwIdPodstDostawca)
                .HasConstraintName("FK_tw__Towar_kh__Kontrahent_PodstawowyDostawca");

            entity.HasOne(d => d.TwIdProducentaNavigation).WithMany(p => p.TwTowarTwIdProducentaNavigations)
                .HasForeignKey(d => d.TwIdProducenta)
                .HasConstraintName("FK_tw__Towar_kh__Kontrahent_Producent");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
