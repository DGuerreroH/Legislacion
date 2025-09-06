using LegislacionAPP.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using static LegislacionAPP.Models.DBModels.CMMIPregunta;

namespace LegislacionAPP.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets (ajusta a tus clases reales)
    public DbSet<Pais> Pais => Set<Pais>();
    public DbSet<Estado> Estado => Set<Estado>();
    public DbSet<AmbitoAplicacion> AmbitoAplicacion => Set<AmbitoAplicacion>();
    public DbSet<Rol> Rol => Set<Rol>();
    public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<Empresa> Empresa => Set<Empresa>();
    public DbSet<UsuarioEmpresa> UsuarioEmpresa => Set<UsuarioEmpresa>();
    public DbSet<TipoElemento> TipoElemento => Set<TipoElemento>();
    public DbSet<Legislacion> Legislacion => Set<Legislacion>();
    public DbSet<SegmentoLegislacion> SegmentoLegislacion => Set<SegmentoLegislacion>();
    public DbSet<CicloAuditoria> CicloAuditoria => Set<CicloAuditoria>();
    public DbSet<EvaluacionSegmento> EvaluacionSegmento => Set<EvaluacionSegmento>();
    public DbSet<Evidencia> Evidencia => Set<Evidencia>();
    public DbSet<Sector> Sector => Set<Sector>();
    public DbSet<CCMICategoria> CCMICategoria => Set<CCMICategoria>();
    public DbSet<CMMIRespuesta> CMMIRespuesta => Set<CMMIRespuesta>();

    public DbSet<CMMIEvaluacion> CMMIEvaluacion => Set<CMMIEvaluacion>();
    public DbSet<CMMIPregunta> CMMIPregunta => Set<CMMIPregunta>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Charset/collation por defecto (MySQL 8 + utf8mb4)
        modelBuilder.HasCharSet("utf8mb4").UseCollation("utf8mb4_general_ci");
        
        modelBuilder.Entity<Empresa>()
            .Property(e => e.nombre).HasMaxLength(200);

        modelBuilder.Entity<CMMIRespuesta>()
            .Property(r => r.valor)    
            .HasPrecision(5, 2);       
        
    }
}
