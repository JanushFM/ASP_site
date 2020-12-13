using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SmallDescription = table.Column<string>(maxLength: 600, nullable: false),
                    BigDescription = table.Column<string>(maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Description", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(maxLength: 60, nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Genre = table.Column<string>(maxLength: 30, nullable: false),
                    Rating = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    DescriptionId = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(nullable: false),
                    Quote = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artist_Description_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Description",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Painting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    ImageName = table.Column<string>(nullable: false),
                    DescriptionId = table.Column<int>(nullable: false),
                    ArtistId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    NumberAvailable = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Painting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Painting_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Painting_Description_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Description",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    PaintingId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    ShippingAddress = table.Column<string>(nullable: true),
                    IsConfirmedByUser = table.Column<bool>(nullable: false),
                    IsReviewedBySailor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Painting_PaintingId",
                        column: x => x.PaintingId,
                        principalTable: "Painting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Description",
                columns: new[] { "Id", "BigDescription", "SmallDescription" },
                values: new object[] { 1, "Vincent Willem van Gogh (Dutch: [ˈvɪnsənt ˈʋɪləm vɑŋ ˈɣɔx] (About this soundlisten);[note 1] 30 March 1853 – 29 July 1890) was a Dutch post-impressionist painter who is among the most famous and influential figures in the history of Western art. In just over a decade, he created about 2,100 artworks, including around 860 oil paintings, most of which date from the last two years of his life. They include landscapes, still lifes, portraits and self-portraits, and are characterised by bold colours and dramatic, impulsive and expressive brushwork that contributed to the foundations of modern art. He was not commercially successful, and his suicide at 37 came after years of mental illness, depression and poverty.  Born into an upper-middle-class family, Van Gogh drew as a child and was serious, quiet, and thoughtful. As a young man he worked as an art dealer, often travelling, but became depressed after he was transferred to London. He turned to religion and spent time as a Protestant missionary in southern Belgium. He drifted in ill health and solitude before taking up painting in 1881, having moved back home with his parents. His younger brother Theo supported him financially, and the two kept up a long correspondence by letter. His early works, mostly still lifes and depictions of peasant labourers, contain few signs of the vivid colour that distinguished his later work. In 1886, he moved to Paris, where he met members of the avant-garde, including Émile Bernard and Paul Gauguin, who were reacting against the Impressionist sensibility. As his work developed he created a new approach to still lifes and local landscapes. His paintings grew brighter in colour as he developed a style that became fully realised during his stay in Arles in the south of France in 1888. During this period he broadened his subject matter to include series of olive trees, wheat fields and sunflowers.  Van Gogh suffered from psychotic episodes and delusions and though he worried about his mental stability, he often neglected his physical health, did not eat properly and drank heavily. His friendship with Gauguin ended after a confrontation with a razor when, in a rage, he severed part of his own left ear. He spent time in psychiatric hospitals, including a period at Saint-Rémy. After he discharged himself and moved to the Auberge Ravoux in Auvers-sur-Oise near Paris, he came under the care of the homeopathic doctor Paul Gachet. His depression continued and on 27 July 1890, Van Gogh shot himself in the chest with a Lefaucheux revolver.[6] He died from his injuries two days later.  Van Gogh was unsuccessful during his lifetime, and was considered a madman and a failure. He became famous after his suicide, and exists in the public imagination as the quintessential misunderstood genius, the artist \"where discourses on madness and creativity converge\".[7] His reputation began to grow in the early 20th century as elements of his painting style came to be incorporated by the Fauves and German Expressionists. He attained widespread critical, commercial and popular success over the ensuing decades, and is remembered as an important but tragic painter, whose troubled personality typifies the romantic ideal of the tortured artist. Today, Van Gogh's works are among the world's most expensive paintings to have ever sold, and his legacy is honoured by a museum in his name, the Van Gogh Museum in Amsterdam, which holds the world's largest collection of his paintings and drawings.", "Vincent Willem van Gogh was a Dutch post-impressionist painter who is among the most famous and influential figures in the history of Western art. In just over a decade, he created about 2,100 artwork" });

            migrationBuilder.InsertData(
                table: "Description",
                columns: new[] { "Id", "BigDescription", "SmallDescription" },
                values: new object[] { 2, "Leonardo di ser Piero da Vinci (Italian: [leoˈnardo di ˌsɛr ˈpjɛːro da (v)ˈvintʃi] (About this soundlisten); 14/15 April 1452[a] – 2 May 1519),[3] known as Leonardo da Vinci (English: /ˌliːəˈnɑːrdoʊ də ˈvɪntʃi, ˌliːoʊˈ-, ˌleɪoʊˈ-/ LEE-ə-NAR-doh də VIN-chee, LEE-oh-, LAY-oh-),[4] was an Italian polymath of the Renaissance whose areas of interest included invention, drawing, painting, sculpture, architecture, science, music, mathematics, engineering, literature, anatomy, geology, astronomy, botany, paleontology, and cartography. He has been variously called the father of palaeontology, ichnology, and architecture, and is widely considered one of the greatest painters of all time (despite perhaps only 15 of his paintings having survived).[b]  Born out of wedlock to a notary, Piero da Vinci, and a peasant woman, Caterina, in Vinci, in the region of Florence, Italy, Leonardo was educated in the studio of the renowned Italian painter Andrea del Verrocchio. Much of his earlier working life was spent in the service of Ludovico il Moro in Milan, and he later worked in Rome, Bologna and Venice. He spent his last three years in France, where he died in 1519.  Leonardo is renowned primarily as a painter. The Mona Lisa is the most famous of his works and the most popular portrait ever made.[5] The Last Supper is the most reproduced religious painting of all time[6] and his Vitruvian Man drawing is regarded as a cultural icon as well.[7] Salvator Mundi was sold for a world record $450.3 million at a Christie's auction in New York, 15 November 2017, the highest price ever paid for a work of art.[8] Leonardo's paintings and preparatory drawings—together with his notebooks, which contain sketches, scientific diagrams, and his thoughts on the nature of painting—compose a contribution to later generations of artists rivalled only by that of his contemporary Michelangelo.[9]  Although he had no formal academic training,[10] many historians and scholars regard Leonardo as the prime exemplar of the \"UniversalGenius\" or \"Renaissance Man\", an individual of \"unquenchable curiosity\" and \"feverishly inventive imagination.\"[6] He is widely considered one of the most diversely talented individuals ever to have lived.[11] According to art historian Helen Gardner, the scope and depth of his interests were without precedent in recorded history, and \"his mind and personality seem to us superhuman, while the man himself mysterious and remote.\"[6] Scholars interpret his view of the world as being based in logic, though the empirical methods he used were unorthodox for his time.[12]  Leonardo is revered for his technological ingenuity. He conceptualized flying machines, a type of armoured fighting vehicle, concentrated solar power, an adding machine,[13] and the double hull. Relatively few of his designs were constructed or even feasible during his lifetime, as the modern scientific approaches to metallurgy and engineering were only in their infancy during the Renaissance. Some of his smaller inventions, however, entered the world of manufacturing unheralded, such as an automated bobbin winder and a machine for testing the tensile strength of wire. He is also sometimes credited with the inventions of the parachute, helicopter, and tank.[14][15] He made substantial discoveries in anatomy, civil engineering, geology, optics, and hydrodynamics, but he did not publish his findings and they had little to no direct influence on subsequent science.[16]", "Leonardo di ser Piero da Vinci, known as Leonardo da Vinci, was an Italian polymath of the Renaissance whose areas of interest included invention, drawing, painting, sculpture, architecture, science," });

            migrationBuilder.InsertData(
                table: "Description",
                columns: new[] { "Id", "BigDescription", "SmallDescription" },
                values: new object[] { 3, "The Mona Lisa (/ˌmoʊnə ˈliːsə/; Italian: Monna Lisa [ˈmɔnna ˈliːza] or La Gioconda [la dʒoˈkonda], French: La Joconde [la ʒɔkɔ̃d]) is a half-length portrait painting by the Italian artist Leonardo da Vinci. It is considered an archetypal masterpiece of the Italian Renaissance,[4][5] and has been described as \"the best known, the most visited, the most written about, the most sung about, the most parodied work of art in the world.\"[6] The painting's novel qualities include the subject's expression, which is frequently described as enigmatic,[7] the monumentality of the composition, the subtle modelling of forms, and the atmospheric illusionism.[8]  The painting is likely of the Italian noblewoman Lisa Gherardini,[9] the wife of Francesco del Giocondo, and is in oil on a white Lombardy poplar panel. It had been believed to have been painted between 1503 and 1506; however, Leonardo may have continued working on it as late as 1517. Recent academic work suggests that it would not have been started before 1513.[10][11][12][13] It was acquired by King Francis I of France and is now the property of the French Republic itself, on permanent display at the Louvre Museum in Paris since 1797.[14]  The Mona Lisa is one of the most valuable paintings in the world. It holds the Guinness World Record for the highest known insurance valuation in history at US$100 million in 1962[15] (equivalent to $650 million in 2018).", "The Mona Lisa is a half-length portrait painting by the Italian artist Leonardo da Vinci. It is considered an archetypal" });

            migrationBuilder.InsertData(
                table: "Description",
                columns: new[] { "Id", "BigDescription", "SmallDescription" },
                values: new object[] { 4, "Although The Starry Night was painted during the day in Van Gogh's ground-floor studio, it would be inaccurate to state that the picture was painted from memory. The view has been identified as the one from his bedroom window, facing east,[1][2][16][17] a view which Van Gogh painted variations of no fewer than twenty-one times,[citation needed] including The Starry Night. \"Through the iron-barred window,\" he wrote to his brother, Theo, around 23 May 1889, \"I can see an enclosed square of wheat . . . above which, in the morning, I watch the sun rise in all its glory\"  Van Gogh depicted the view at different times of the day and under various weather conditions, including sunrise, moonrise, sunshine-filled days, overcast days, windy days, and one day with rain. While the hospital staff did not allow Van Gogh to paint in his bedroom, he was able there to make sketches in ink or charcoal on paper; eventually, he would base newer variations on previous versions. The pictorial element uniting all of these paintings is the diagonal line coming in from the right depicting the low rolling hills of the Alpilles mountains. In fifteen of the twenty-one versions, cypress trees are visible beyond the far wall enclosing the wheat field. Van Gogh telescoped the view in six of these[vague] paintings, most notably in F717 Wheat Field with Cypresses and The Starry Night, bringing the trees closer to the picture plane.[citation needed]  One of the first paintings of the view was F611 Mountainous Landscape Behind Saint-Rémy, now in Copenhagen. Van Gogh made a number of sketches for the painting, of which F1547 The Enclosed Wheatfield After a Storm is typical. It is unclear whether the painting was made in his studio or outside. In his June 9 letter describing it, he mentions he had been working outside for a few days.[18][19][L 3][14] Van Gogh described the second of the two landscapes he mentions he was working on, in a letter to his sister Wil on 16 June 1889.[18][L 4] This is F719 Green Field, now in Prague, and the first painting at the asylum he definitely painted en plein air.[18] F1548 Wheatfield, Saint-Rémy de Provence, now in New York, is a study for it. Two days later, Vincent wrote Theo that he had painted \"a starry sky\".[20][L 1]  The Starry Night is the only nocturne in the series of views from his bedroom window. In early June, Vincent wrote to Theo, \"This morning I saw the countryside from my window a long time before sunrise with nothing but the morning star, which looked very big\" Researchers have determined that Venus was indeed visible at dawn in Provence in the spring of 1889, and was at that time nearly as bright as possible. So the brightest \"star\" in the painting, just to the viewer's right of the cypress tree, is actually Venus.[14][16]  The Moon is stylized, as astronomical records indicate that it actually was waning gibbous at the time Van Gogh painted the picture,[14] and even if the phase of the Moon had been its waning crescent at the time, Van Gogh's Moon would not have been astronomically correct. (For other interpretations of the Moon, see below.) The one pictorial element that was definitely not visible from Van Gogh's cell is the village,[21] which is based on a sketch F1541v made from a hillside above the village of Saint-Rémy.[3] Pickvance thought F1541v was done later, and the steeple more Dutch than Provençal, a conflation of several Van Gogh had painted and drawn in his Nuenen period, and thus the first of his \"reminisces of the North\" he was to paint and draw early the following year.[1] Hulsker thought a landscape on the reverse F1541r was also a study for the painting.", "The Starry Night is an oil on canvas by Dutch post-impressionist painter Vincent van Gogh. Painted in June 1889, it descri" });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 1, "Romantic Comedy", 7.99m, "R", new DateTime(1989, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "When Harry Met Sally" });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 2, "Comedy", 8.99m, "R", new DateTime(1984, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ghostbusters " });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 3, "Comedy", 9.99m, "R", new DateTime(1986, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ghostbusters 2" });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 4, "Western", 3.99m, "R", new DateTime(1959, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rio Bravo" });

            migrationBuilder.InsertData(
                table: "Artist",
                columns: new[] { "Id", "DescriptionId", "ImageName", "Name", "Quote" },
                values: new object[] { 1, 1, "Vincent_van_Gogh__portrait.jpg", "Vincent van Gogh", "quote" });

            migrationBuilder.InsertData(
                table: "Artist",
                columns: new[] { "Id", "DescriptionId", "ImageName", "Name", "Quote" },
                values: new object[] { 2, 2, "Leonardo_da_Vinci_portrait_Ti9m3nK.jpg", "Leonardo da Vinci", "Painting is poetry that is seen rather than felt, and poetry is painting that is felt rather than seen" });

            migrationBuilder.InsertData(
                table: "Painting",
                columns: new[] { "Id", "ArtistId", "DescriptionId", "ImageName", "Name", "NumberAvailable", "Price" },
                values: new object[] { 2, 1, 4, "Starry_Night.jpg", "Starry Night", 6, 4656 });

            migrationBuilder.InsertData(
                table: "Painting",
                columns: new[] { "Id", "ArtistId", "DescriptionId", "ImageName", "Name", "NumberAvailable", "Price" },
                values: new object[] { 1, 2, 3, "mono_lisa.jpg", "Mona Lisa", 10, 1465 });

            migrationBuilder.CreateIndex(
                name: "IX_Artist_DescriptionId",
                table: "Artist",
                column: "DescriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_AppUserId",
                table: "Order",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaintingId",
                table: "Order",
                column: "PaintingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Painting_ArtistId",
                table: "Painting",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Painting_DescriptionId",
                table: "Painting",
                column: "DescriptionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Painting");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Description");
        }
    }
}
