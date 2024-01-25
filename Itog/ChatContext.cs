using Microsoft.EntityFrameworkCore;

namespace Itog
{
    internal class ChatContext : DbContext
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatContext()
        {

        }

        public ChatContext(DbContextOptions<ChatContext> dbc) : base(dbc)
        {
            
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.; Database=myDataBase; Trusted_Connection=True;")
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id).HasName("user_pkey");
                entity.HasIndex(x => x.FullName).IsUnique();

                entity.Property(e => e.FullName)
                .HasColumnName("FullName")
                .HasMaxLength(255)
                .IsRequired();
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.HasKey(x => x.MessageId).HasName("message_pkey");

                entity.Property(e => e.MessageId).HasColumnName("id");
                entity.Property(e => e.Text).HasColumnName("Text");
                entity.Property(e => e.DateSend).HasColumnName("message_data");
                entity.Property(e => e.isSent).HasColumnName("is_send");

                entity.HasOne(m => m.UserTo)
                .WithMany(m => m.MessagesTo)
                .HasForeignKey(m => m.UserToId).HasConstraintName("messageToUserFk"); ;

                entity.HasOne(m => m.UserFrom)
                .WithMany(m => m.MessagesFrom)
                .HasForeignKey(m => m.UserFromId).HasConstraintName("messageFromUserFk");
            });
            
        }
        
    }
}
