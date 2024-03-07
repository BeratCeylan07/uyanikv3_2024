using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace uyanikv3.Models;

public partial class U1626744Db60AContext : DbContext
{
    public U1626744Db60AContext()
    {
    }

    public U1626744Db60AContext(DbContextOptions<U1626744Db60AContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anakategoriler> Anakategorilers { get; set; }

    public virtual DbSet<Bildirimler> Bildirimlers { get; set; }

    public virtual DbSet<DenemeSeanslar> DenemeSeanslars { get; set; }

    public virtual DbSet<DenemeStoklar> DenemeStoklars { get; set; }

    public virtual DbSet<Denemeler> Denemelers { get; set; }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<Kurumkutuphaneler> Kurumkutuphanelers { get; set; }

    public virtual DbSet<Kutuphanetoken> Kutuphanetokens { get; set; }

    public virtual DbSet<Kutuphaneuyelikpaketler> Kutuphaneuyelikpaketlers { get; set; }

    public virtual DbSet<Kutuphaneuyelikpakettanimlamalar> Kutuphaneuyelikpakettanimlamalars { get; set; }

    public virtual DbSet<Merkezsubeler> Merkezsubelers { get; set; }

    public virtual DbSet<Ogrbildirimlog> Ogrbildirimlogs { get; set; }

    public virtual DbSet<OgrenciOdemeler> OgrenciOdemelers { get; set; }

    public virtual DbSet<Ogrenciler> Ogrencilers { get; set; }

    public virtual DbSet<Ogrtoken> Ogrtokens { get; set; }

    public virtual DbSet<Okullar> Okullars { get; set; }

    public virtual DbSet<Raporlar> Raporlars { get; set; }

    public virtual DbSet<SeansOgrSet> SeansOgrSets { get; set; }

    public virtual DbSet<SeansPaket> SeansPakets { get; set; }

    public virtual DbSet<SeansPaketTanim> SeansPaketTanims { get; set; }

    public virtual DbSet<Seansbildirimlog> Seansbildirimlogs { get; set; }

    public virtual DbSet<Systmuser> Systmusers { get; set; }

    public virtual DbSet<UserSubsubeSet> UserSubsubeSets { get; set; }

    public virtual DbSet<Uyegirishareketler> Uyegirishareketlers { get; set; }

    public virtual DbSet<Uyegirisikramhareket> Uyegirisikramharekets { get; set; }

    public virtual DbSet<Uyelikpaketikramtanimlamalar> Uyelikpaketikramtanimlamalars { get; set; }

    public virtual DbSet<Wplink> Wplinks { get; set; }

    public virtual DbSet<Yayinlar> Yayinlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=94.73.145.160;database=u1626744_db60A;user=u1626744_user60A;password=x@9Qe1-x3H2_Q-nW", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.16-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_turkish_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Anakategoriler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anakategoriler");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.AnaKategoriBaslik)
                .HasMaxLength(100)
                .HasColumnName("anaKategoriBaslik");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.Anakategorilers)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("anakategoriler_ibfk_1");
        });

        modelBuilder.Entity<Bildirimler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bildirimler");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Baslik)
                .HasMaxLength(100)
                .HasColumnName("baslik");
            entity.Property(e => e.Durum)
                .HasColumnType("int(11)")
                .HasColumnName("durum");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.Mesaj)
                .HasMaxLength(250)
                .HasColumnName("mesaj");
            entity.Property(e => e.Tarih)
                .HasColumnType("datetime")
                .HasColumnName("tarih");
        });

        modelBuilder.Entity<DenemeSeanslar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("deneme_seanslar");

            entity.HasIndex(e => e.DenemeId, "deneme_ID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.DenemeId)
                .HasColumnType("int(11)")
                .HasColumnName("deneme_ID");
            entity.Property(e => e.Durum)
                .HasColumnType("int(11)")
                .HasColumnName("durum");
            entity.Property(e => e.Kontenjan)
                .HasColumnType("int(11)")
                .HasColumnName("kontenjan");
            entity.Property(e => e.Saat)
                .HasMaxLength(10)
                .HasColumnName("saat");
            entity.Property(e => e.SeansUcret).HasColumnName("seans_Ucret");
            entity.Property(e => e.Tarih)
                .HasMaxLength(6)
                .HasColumnName("tarih");

            entity.HasOne(d => d.Deneme).WithMany(p => p.DenemeSeanslars)
                .HasForeignKey(d => d.DenemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deneme_seanslar_ibfk_1");
        });

        modelBuilder.Entity<DenemeStoklar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("deneme_stoklar");

            entity.HasIndex(e => e.DenemeId, "deneme_ID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adet)
                .HasColumnType("int(11)")
                .HasColumnName("adet");
            entity.Property(e => e.DenemeId)
                .HasColumnType("int(11)")
                .HasColumnName("deneme_ID");
            entity.Property(e => e.StokType)
                .HasColumnType("int(11)")
                .HasColumnName("stok_Type");
            entity.Property(e => e.Tarih)
                .HasColumnType("datetime")
                .HasColumnName("tarih");

            entity.HasOne(d => d.Deneme).WithMany(p => p.DenemeStoklars)
                .HasForeignKey(d => d.DenemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deneme_stoklar_ibfk_1");
        });

        modelBuilder.Entity<Denemeler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("denemeler");

            entity.HasIndex(e => e.KategoriId, "kategori_ID");

            entity.HasIndex(e => e.YayinId, "yayin_ID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.DenemeBaslik)
                .HasMaxLength(50)
                .HasColumnName("deneme_Baslik");
            entity.Property(e => e.GirisFiyat).HasColumnName("giris_Fiyat");
            entity.Property(e => e.KategoriId)
                .HasColumnType("int(11)")
                .HasColumnName("kategori_ID");
            entity.Property(e => e.YayinId)
                .HasColumnType("int(11)")
                .HasColumnName("yayin_ID");

            entity.HasOne(d => d.Kategori).WithMany(p => p.Denemelers)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("denemeler_ibfk_3");

            entity.HasOne(d => d.Yayin).WithMany(p => p.Denemelers)
                .HasForeignKey(d => d.YayinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("denemeler_ibfk_2");
        });

        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kategoriler");

            entity.HasIndex(e => e.KatId, "katID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(100)
                .HasColumnName("aciklama");
            entity.Property(e => e.AltKategoriBaslik)
                .HasMaxLength(50)
                .HasColumnName("alt_kategori_Baslik");
            entity.Property(e => e.KatId)
                .HasColumnType("int(11)")
                .HasColumnName("katID");

            entity.HasOne(d => d.Kat).WithMany(p => p.Kategorilers)
                .HasForeignKey(d => d.KatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kategoriler_ibfk_1");
        });

        modelBuilder.Entity<Kurumkutuphaneler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kurumkutuphaneler");

            entity.HasIndex(e => e.MerkezId, "merkezID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adres)
                .HasMaxLength(200)
                .HasColumnName("adres");
            entity.Property(e => e.Adsoyad)
                .HasMaxLength(70)
                .HasColumnName("ADSOYAD");
            entity.Property(e => e.Banka)
                .HasMaxLength(70)
                .HasColumnName("BANKA");
            entity.Property(e => e.Eposta)
                .HasMaxLength(75)
                .HasColumnName("eposta");
            entity.Property(e => e.Iban)
                .HasMaxLength(70)
                .HasColumnName("IBAN");
            entity.Property(e => e.Il)
                .HasMaxLength(100)
                .HasColumnName("il");
            entity.Property(e => e.Ilce)
                .HasMaxLength(100)
                .HasColumnName("ilce");
            entity.Property(e => e.KutuphaneBaslik)
                .HasMaxLength(250)
                .HasColumnName("kutuphane_baslik");
            entity.Property(e => e.MerkezId)
                .HasColumnType("int(11)")
                .HasColumnName("merkezID");
            entity.Property(e => e.Onkayitexplanation)
                .HasMaxLength(500)
                .HasColumnName("ONKAYITEXPLANATION");
            entity.Property(e => e.Tel)
                .HasMaxLength(75)
                .HasColumnName("tel");
            entity.Property(e => e.Tel2)
                .HasMaxLength(75)
                .HasColumnName("tel2");

            entity.HasOne(d => d.Merkez).WithMany(p => p.Kurumkutuphanelers)
                .HasForeignKey(d => d.MerkezId)
                .HasConstraintName("kurumkutuphaneler_ibfk_1");
        });

        modelBuilder.Entity<Kutuphanetoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kutuphanetokens");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.Token)
                .HasMaxLength(250)
                .HasColumnName("token");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.InverseKutuphane)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kutuphanetokens_ibfk_1");
        });

        modelBuilder.Entity<Kutuphaneuyelikpaketler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kutuphaneuyelikpaketler");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.GecerlilikToplamGun)
                .HasColumnType("int(11)")
                .HasColumnName("gecerlilikToplamGun");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.PaketBaslik)
                .HasMaxLength(150)
                .HasColumnName("paketBaslik");
            entity.Property(e => e.PaketDurum)
                .HasColumnType("int(11)")
                .HasColumnName("paketDurum");
            entity.Property(e => e.ToplamGirisHak)
                .HasColumnType("int(11)")
                .HasColumnName("toplamGirisHak");
            entity.Property(e => e.Ucret).HasColumnName("ucret");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.Kutuphaneuyelikpaketlers)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kutuphaneuyelikpaketler_ibfk_1");
        });

        modelBuilder.Entity<Kutuphaneuyelikpakettanimlamalar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kutuphaneuyelikpakettanimlamalar");

            entity.HasIndex(e => e.OgrId, "ogrID");

            entity.HasIndex(e => e.PaketId, "paketID");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.BaslangicTarih)
                .HasColumnType("datetime")
                .HasColumnName("baslangicTarih");
            entity.Property(e => e.BitisTarih)
                .HasColumnType("datetime")
                .HasColumnName("bitisTarih");
            entity.Property(e => e.OgrId)
                .HasColumnType("int(11)")
                .HasColumnName("ogrID");
            entity.Property(e => e.PaketId)
                .HasColumnType("int(11)")
                .HasColumnName("paketID");
            entity.Property(e => e.Tarih)
                .HasColumnType("datetime")
                .HasColumnName("tarih");

            entity.HasOne(d => d.Ogr).WithMany(p => p.Kutuphaneuyelikpakettanimlamalars)
                .HasForeignKey(d => d.OgrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kutuphaneuyelikpakettanimlamalar_ibfk_1");

            entity.HasOne(d => d.Paket).WithMany(p => p.Kutuphaneuyelikpakettanimlamalars)
                .HasForeignKey(d => d.PaketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kutuphaneuyelikpakettanimlamalar_ibfk_2");
        });

        modelBuilder.Entity<Merkezsubeler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("merkezsubeler");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Il)
                .HasMaxLength(150)
                .HasColumnName("IL");
            entity.Property(e => e.MerkezSubeAdi)
                .HasMaxLength(100)
                .HasColumnName("MERKEZ_SUBE_ADI");
        });

        modelBuilder.Entity<Ogrbildirimlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ogrbildirimlog");

            entity.HasIndex(e => e.BildirimId, "bildirimID");

            entity.HasIndex(e => e.OgrId, "ogrID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.BildirimId)
                .HasColumnType("int(11)")
                .HasColumnName("bildirimID");
            entity.Property(e => e.LogTarih)
                .HasColumnType("datetime")
                .HasColumnName("logTarih");
            entity.Property(e => e.OgrId)
                .HasColumnType("int(11)")
                .HasColumnName("ogrID");

            entity.HasOne(d => d.Bildirim).WithMany(p => p.Ogrbildirimlogs)
                .HasForeignKey(d => d.BildirimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrbildirimlog_ibfk_1");

            entity.HasOne(d => d.Ogr).WithMany(p => p.Ogrbildirimlogs)
                .HasForeignKey(d => d.OgrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrbildirimlog_ibfk_2");
        });

        modelBuilder.Entity<OgrenciOdemeler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ogrenci_odemeler");

            entity.HasIndex(e => e.OgrId, "ogrID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Durum)
                .HasColumnType("int(11)")
                .HasColumnName("durum");
            entity.Property(e => e.OgrId)
                .HasColumnType("int(11)")
                .HasColumnName("ogrID");
            entity.Property(e => e.Tarih)
                .HasColumnType("datetime")
                .HasColumnName("tarih");
            entity.Property(e => e.Tutar).HasColumnName("tutar");

            entity.HasOne(d => d.Ogr).WithMany(p => p.OgrenciOdemelers)
                .HasForeignKey(d => d.OgrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrenci_odemeler_ibfk_1");
        });

        modelBuilder.Entity<Ogrenciler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ogrenciler");

            entity.HasIndex(e => e.KategoriId, "kategoriID");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.HasIndex(e => e.OkulId, "okulID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.Durum)
                .HasColumnType("int(11)")
                .HasColumnName("durum");
            entity.Property(e => e.KategoriId)
                .HasColumnType("int(11)")
                .HasColumnName("kategoriID");
            entity.Property(e => e.Ktarih)
                .HasColumnType("datetime")
                .HasColumnName("KTarih");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.KutuphaneUye)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneUye");
            entity.Property(e => e.OkulId)
                .HasColumnType("int(11)")
                .HasColumnName("okulID");
            entity.Property(e => e.Qrasc)
                .HasMaxLength(1000)
                .HasColumnName("qrasc");
            entity.Property(e => e.Sifre)
                .HasMaxLength(100)
                .HasColumnName("sifre");
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .HasColumnName("soyad");
            entity.Property(e => e.Telefon)
                .HasMaxLength(50)
                .HasColumnName("telefon");

            entity.HasOne(d => d.Kategori).WithMany(p => p.Ogrencilers)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrenciler_ibfk_1");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.Ogrencilers)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrenciler_ibfk_3");

            entity.HasOne(d => d.Okul).WithMany(p => p.Ogrencilers)
                .HasForeignKey(d => d.OkulId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrenciler_ibfk_2");
        });

        modelBuilder.Entity<Ogrtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ogrtokens");

            entity.HasIndex(e => e.OgrId, "ogrID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.OgrId)
                .HasColumnType("int(11)")
                .HasColumnName("ogrID");
            entity.Property(e => e.Token)
                .HasMaxLength(1000)
                .HasColumnName("token");

            entity.HasOne(d => d.Ogr).WithMany(p => p.Ogrtokens)
                .HasForeignKey(d => d.OgrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ogrtokens_ibfk_1");
        });

        modelBuilder.Entity<Okullar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("okullar");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.OkulBaslik).HasMaxLength(100);

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.Okullars)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("okullar_ibfk_1");
        });

        modelBuilder.Entity<Raporlar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("raporlar");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.D1).HasColumnType("datetime");
            entity.Property(e => e.D2).HasColumnType("datetime");
            entity.Property(e => e.HedefKar).HasColumnName("hedefKar");
            entity.Property(e => e.HedefKarToplamKarFark).HasColumnName("hedefKarToplamKarFark");
            entity.Property(e => e.IskartaDenemeZarar).HasColumnName("iskartaDenemeZarar");
            entity.Property(e => e.KotuDeneme)
                .HasMaxLength(75)
                .HasColumnName("kotuDeneme");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.RaporAy).HasMaxLength(50);
            entity.Property(e => e.SampiyonDeneme)
                .HasMaxLength(75)
                .HasColumnName("sampiyonDeneme");
            entity.Property(e => e.Tarih)
                .HasColumnType("datetime")
                .HasColumnName("tarih");
            entity.Property(e => e.ToplamCiro).HasColumnName("toplamCiro");
            entity.Property(e => e.ToplamDenemeSeans)
                .HasColumnType("int(11)")
                .HasColumnName("toplamDenemeSeans");
            entity.Property(e => e.ToplamKar).HasColumnName("toplamKar");
            entity.Property(e => e.ToplamMasraf).HasColumnName("toplamMasraf");
            entity.Property(e => e.ToplamSinavaGirenOgr)
                .HasColumnType("int(11)")
                .HasColumnName("toplamSinavaGirenOgr");
            entity.Property(e => e.ToplamStokCikislar)
                .HasColumnType("int(11)")
                .HasColumnName("toplamStokCikislar");
            entity.Property(e => e.ToplamStokGirisler)
                .HasColumnType("int(11)")
                .HasColumnName("toplamStokGirisler");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.Raporlars)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("raporlar_ibfk_1");
        });

        modelBuilder.Entity<SeansOgrSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("seans_ogr_set");

            entity.HasIndex(e => e.OgrId, "ogr_ID");

            entity.HasIndex(e => e.SeansId, "seans_ID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(75)
                .HasColumnName("aciklama");
            entity.Property(e => e.Durum)
                .HasColumnType("int(11)")
                .HasColumnName("durum");
            entity.Property(e => e.OgrId)
                .HasColumnType("int(11)")
                .HasColumnName("ogr_ID");
            entity.Property(e => e.Qr)
                .HasMaxLength(70)
                .HasColumnName("QR");
            entity.Property(e => e.SeansId)
                .HasColumnType("int(11)")
                .HasColumnName("seans_ID");
            entity.Property(e => e.SeansKayitTarih)
                .HasColumnType("datetime")
                .HasColumnName("seans_Kayit_Tarih");

            entity.HasOne(d => d.Ogr).WithMany(p => p.SeansOgrSets)
                .HasForeignKey(d => d.OgrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seans_ogr_set_ibfk_1");

            entity.HasOne(d => d.Seans).WithMany(p => p.SeansOgrSets)
                .HasForeignKey(d => d.SeansId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seans_ogr_set_ibfk_2");
        });

        modelBuilder.Entity<SeansPaket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("seans_paket");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adet)
                .HasColumnType("int(11)")
                .HasColumnName("adet");
            entity.Property(e => e.Fiyat).HasColumnName("fiyat");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.PaketAdi)
                .HasMaxLength(100)
                .HasColumnName("paket_adi");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.SeansPakets)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seans_paket_ibfk_1");
        });

        modelBuilder.Entity<SeansPaketTanim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("seans_paket_tanim");

            entity.HasIndex(e => e.Paketid, "PAKET");

            entity.HasIndex(e => e.Seansid, "SEANSID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Paketid)
                .HasColumnType("int(11)")
                .HasColumnName("PAKETID");
            entity.Property(e => e.Seansid)
                .HasColumnType("int(11)")
                .HasColumnName("SEANSID");

            entity.HasOne(d => d.Paket).WithMany(p => p.SeansPaketTanims)
                .HasForeignKey(d => d.Paketid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seans_paket_tanim_ibfk_1");

            entity.HasOne(d => d.Seans).WithMany(p => p.SeansPaketTanims)
                .HasForeignKey(d => d.Seansid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seans_paket_tanim_ibfk_2");
        });

        modelBuilder.Entity<Seansbildirimlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("seansbildirimlog");

            entity.HasIndex(e => e.BildirimId, "bildirimID");

            entity.HasIndex(e => e.SeansId, "seansID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.BildirimId)
                .HasColumnType("int(11)")
                .HasColumnName("bildirimID");
            entity.Property(e => e.LogTarih)
                .HasColumnType("datetime")
                .HasColumnName("logTarih");
            entity.Property(e => e.SeansId)
                .HasColumnType("int(11)")
                .HasColumnName("seansID");

            entity.HasOne(d => d.Bildirim).WithMany(p => p.Seansbildirimlogs)
                .HasForeignKey(d => d.BildirimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seansbildirimlog_ibfk_1");

            entity.HasOne(d => d.Seans).WithMany(p => p.Seansbildirimlogs)
                .HasForeignKey(d => d.SeansId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seansbildirimlog_ibfk_2");
        });

        modelBuilder.Entity<Systmuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("systmuser");

            entity.HasIndex(e => e.SubeId, "subeID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Auth)
                .HasColumnType("int(11)")
                .HasColumnName("auth");
            entity.Property(e => e.KAd)
                .HasMaxLength(50)
                .HasColumnName("kAd");
            entity.Property(e => e.KPass)
                .HasMaxLength(50)
                .HasColumnName("kPass");
            entity.Property(e => e.SubeId)
                .HasColumnType("int(11)")
                .HasColumnName("subeID");
            entity.Property(e => e.Subsubeuser)
                .HasColumnType("int(11)")
                .HasColumnName("subsubeuser");

            entity.HasOne(d => d.Sube).WithMany(p => p.Systmusers)
                .HasForeignKey(d => d.SubeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("systmuser_ibfk_1");
        });

        modelBuilder.Entity<UserSubsubeSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_subsube_set");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Subsubeid)
                .HasColumnType("int(11)")
                .HasColumnName("subsubeid");
            entity.Property(e => e.Userid)
                .HasColumnType("int(11)")
                .HasColumnName("userid");
        });

        modelBuilder.Entity<Uyegirishareketler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("uyegirishareketler");

            entity.HasIndex(e => e.UyeId, "uyeID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Saat)
                .HasMaxLength(75)
                .HasColumnName("saat");
            entity.Property(e => e.Tarih)
                .HasColumnType("datetime")
                .HasColumnName("tarih");
            entity.Property(e => e.UyeId)
                .HasColumnType("int(11)")
                .HasColumnName("uyeID");

            entity.HasOne(d => d.Uye).WithMany(p => p.Uyegirishareketlers)
                .HasForeignKey(d => d.UyeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uyegirishareketler_ibfk_1");
        });

        modelBuilder.Entity<Uyegirisikramhareket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("uyegirisikramhareket");

            entity.HasIndex(e => e.IkramId, "ikramID");

            entity.HasIndex(e => e.UyeGirisId, "uyeGirisID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adet)
                .HasColumnType("int(11)")
                .HasColumnName("adet");
            entity.Property(e => e.Durum)
                .HasColumnType("int(11)")
                .HasColumnName("durum");
            entity.Property(e => e.IkramId)
                .HasColumnType("int(11)")
                .HasColumnName("ikramID");
            entity.Property(e => e.Saat).HasMaxLength(10);
            entity.Property(e => e.Tarih).HasColumnType("datetime");
            entity.Property(e => e.UyeGirisId)
                .HasColumnType("int(11)")
                .HasColumnName("uyeGirisID");

            entity.HasOne(d => d.Ikram).WithMany(p => p.Uyegirisikramharekets)
                .HasForeignKey(d => d.IkramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uyegirisikramhareket_ibfk_1");

            entity.HasOne(d => d.UyeGiris).WithMany(p => p.Uyegirisikramharekets)
                .HasForeignKey(d => d.UyeGirisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uyegirisikramhareket_ibfk_2");
        });

        modelBuilder.Entity<Uyelikpaketikramtanimlamalar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("uyelikpaketikramtanimlamalar");

            entity.HasIndex(e => new { e.Id, e.PaketId }, "Id");

            entity.HasIndex(e => e.PaketId, "paketıD");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Adet)
                .HasColumnType("int(11)")
                .HasColumnName("adet");
            entity.Property(e => e.IkramBaslik)
                .HasMaxLength(150)
                .HasColumnName("ikramBaslik");
            entity.Property(e => e.PaketId)
                .HasColumnType("int(11)")
                .HasColumnName("paketID");

            entity.HasOne(d => d.Paket).WithMany(p => p.Uyelikpaketikramtanimlamalars)
                .HasForeignKey(d => d.PaketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uyelikpaketikramtanimlamalar_ibfk_1");
        });

        modelBuilder.Entity<Wplink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wplinks");

            entity.HasIndex(e => e.KatId, "katID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.KatId)
                .HasColumnType("int(11)")
                .HasColumnName("katID");
            entity.Property(e => e.Link).HasMaxLength(250);
            entity.Property(e => e.LinkBaslik).HasMaxLength(150);

            entity.HasOne(d => d.Kat).WithMany(p => p.Wplinks)
                .HasForeignKey(d => d.KatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("wplinks_ibfk_1");
        });

        modelBuilder.Entity<Yayinlar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("yayinlar");

            entity.HasIndex(e => e.KutuphaneId, "kutuphaneID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(75)
                .HasColumnName("aciklama");
            entity.Property(e => e.KutuphaneId)
                .HasColumnType("int(11)")
                .HasColumnName("kutuphaneID");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .HasColumnName("logo");
            entity.Property(e => e.YayinBaslik)
                .HasMaxLength(75)
                .HasColumnName("yayin_Baslik");

            entity.HasOne(d => d.Kutuphane).WithMany(p => p.Yayinlars)
                .HasForeignKey(d => d.KutuphaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yayinlar_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
