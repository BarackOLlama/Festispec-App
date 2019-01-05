namespace FSBeheer.Migrations
{
    using BCrypt.Net;
    using FSBeheer.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CustomFSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CustomFSContext context)
        {
            context.Answers.RemoveRange(context.Answers);
            context.Questions.RemoveRange(context.Questions);
            context.QuestionTypes.RemoveRange(context.QuestionTypes);
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('dbo.QuestionTypes', RESEED, 0)");
            context.Questionnaires.RemoveRange(context.Questionnaires);
            context.Inspections.RemoveRange(context.Inspections);
            context.InspectionDates.RemoveRange(context.InspectionDates);
            context.Inspectors.RemoveRange(context.Inspectors);
            context.Availabilities.RemoveRange(context.Availabilities);
            context.Accounts.RemoveRange(context.Accounts);
            context.Roles.RemoveRange(context.Roles);
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('dbo.Roles', RESEED, 0)");
            context.Contacts.RemoveRange(context.Contacts);
            context.Customers.RemoveRange(context.Customers);
            context.Events.RemoveRange(context.Events);
            context.Statuses.RemoveRange(context.Statuses);
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('dbo.Status', RESEED, 0)");

            var roles = new List<Role>
            {
                new Role() { Content = "Salesmedewerker" },
                new Role() { Content = "Operationele medewerker" },
                new Role() { Content = "Inspecteur" }
            };
            context.Roles.AddRange(roles);

            var questiontypes = new List<QuestionType>
            {
                new QuestionType() { Name = "Open Vraag" },
                new QuestionType() { Name = "Multiple Choice vraag" },
                new QuestionType() { Name = "Open Tabelvraag" },
                new QuestionType() { Name = "Multiple Choice Tabelvraag" }
            };
            context.QuestionTypes.AddRange(questiontypes);

            string bartSalt = BCrypt.GenerateSalt();
            string phiSalt = BCrypt.GenerateSalt();
            string cjSalt = BCrypt.GenerateSalt();
            string evertSalt = BCrypt.GenerateSalt();
            string mitchSalt = BCrypt.GenerateSalt();

            var accounts = new List<Account>
            {

                new Account()
                {
                    Username = "bkoevoets@gmail.com",
                    Password = BCrypt.HashPassword("bartswachtwoord", bartSalt),
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur"),
                    Salt = bartSalt,
                    IsAdmin = true,
                    IsDeleted = false
                },
                new Account()
                {
                    Username = "pnguyen@gmail.com",
                    Password = BCrypt.HashPassword("phiswachtwoord", phiSalt),
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur"),
                    Salt = phiSalt,
                    IsAdmin = true,
                    IsDeleted = false
                },
                new Account()
                {
                    Username = "clancaster@gmail.com",
                    Password = BCrypt.HashPassword("cjswachtwoord", cjSalt),
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur"),
                    Salt = cjSalt,
                    IsAdmin = true,
                    IsDeleted = false
                },
                new Account()
                {
                    Username = "earends@gmail.com",
                    Password = BCrypt.HashPassword("evertswachtwoord", evertSalt),
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur"),
                    Salt = evertSalt,
                    IsAdmin = true,
                    IsDeleted = false
                },
                new Account()
                {
                    Username = "mitchadmin",
                    Password = BCrypt.HashPassword("password", mitchSalt),
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur"),
                    Salt = mitchSalt,
                    IsAdmin = true,
                    IsDeleted = false
                }
            };
            context.Accounts.AddRange(accounts);

            var inspectors = new List<Inspector>
            {
                new Inspector()
                {
                    Name = "Bart Koevoets",
                    Address = "Bartsadres",
                    City = "Bartsstad",
                    Zipcode = "1234BK",
                    Email = "bkoevoets@gmail.com",
                    PhoneNumber = "0612345678",
                    CertificationDate = new DateTime(2018, 1, 1),
                    InvalidDate = new DateTime(2019, 1, 1),
                    BankNumber = "NL01INGB0123456789",
                    Account = accounts[0],
                    IsDeleted = false
                },
                new Inspector()
                {
                    Name = "Phi Nguyen",
                    Address = "Phisadres",
                    City = "Phisstad",
                    Zipcode = "1234PN",
                    Email = "pnguyen@gmail.com",
                    PhoneNumber = "0612345678",
                    CertificationDate = new DateTime(2018, 1, 1),
                    InvalidDate = new DateTime(2019, 1, 1),
                    BankNumber = "NL01INGB0123456789",
                    Account = accounts[1],
                    IsDeleted = false
                },
                new Inspector()
                {
                    Name = "Curwen Lancaster",
                    Address = "Curwensadres",
                    City = "Curwensstad",
                    Zipcode = "1234CL",
                    Email = "clancaster@gmail.com",
                    PhoneNumber = "0612345678",
                    CertificationDate = new DateTime(2018, 1, 1),
                    InvalidDate = new DateTime(2019, 1, 1),
                    BankNumber = "NL01INGB0123456789",
                    Account = accounts[2],
                    IsDeleted = false
                },
                new Inspector()
                {
                    Name = "Evert Arends",
                    Address = "Evertsadres",
                    City = "Evertsstad",
                    Zipcode = "1234EA",
                    Email = "earends@gmail.com",
                    PhoneNumber = "0612345678",
                    CertificationDate = new DateTime(2018, 1, 1),
                    InvalidDate = new DateTime(2019, 1, 1),
                    BankNumber = "NL01INGB0123456789",
                    Account = accounts[3],
                    IsDeleted = false
                }
            };
            context.Inspectors.AddRange(inspectors);

            var availabilities = new List<Availability>
            {
                new Availability()
                {
                    Inspector = inspectors[0],
                    Date = new DateTime(2018, 3, 1),
                    Scheduled = false,
                    IsDeleted = false
                },
                new Availability()
                {
                    Inspector = inspectors[0],
                    Date = new DateTime(2018, 3, 5),
                    Scheduled = true,
                    ScheduleStartTime = new TimeSpan(22, 0, 0),
                    ScheduleEndTime = new TimeSpan(3, 0, 0),
                    IsDeleted = false
                },
                new Availability()
                {
                    Inspector = inspectors[1],
                    Date = new DateTime(2018, 12, 3),
                    Scheduled = false,
                    IsDeleted = false
                },
                new Availability()
                {
                    Inspector = inspectors[1],
                    Date = new DateTime(2018, 11, 21),
                    Scheduled = true,
                    ScheduleStartTime = new TimeSpan(20, 0, 0),
                    ScheduleEndTime = new TimeSpan(23, 0, 0),
                    IsDeleted = false
                },
                new Availability()
                {
                    Inspector = inspectors[2],
                    Date = new DateTime(2018, 12, 20),
                    Scheduled = false,
                    IsDeleted = false
                },
                new Availability()
                {
                    Inspector = inspectors[3],
                    Date = new DateTime(2018, 12, 10),
                    Scheduled = false,
                    IsDeleted = false
                },
                new Availability()
                {
                    Inspector = inspectors[3],
                    Date = new DateTime(2018, 12, 5),
                    Scheduled = true,
                    ScheduleStartTime = new TimeSpan(20, 0, 0),
                    ScheduleEndTime = new TimeSpan(23, 0, 0),
                    IsDeleted = false
                }
            };
            context.Availabilities.AddRange(availabilities);

            var statuses = new List<Status>
            {
                new Status()
                {
                    StatusName = "Plan"
                },
                new Status()
                {
                    StatusName = "Offerte verstuurd"
                },
                new Status()
                {
                    StatusName = "Pre-planning"
                },
                new Status()
                {
                    StatusName = "Ingepland"
                },
                new Status()
                {
                    StatusName = "Inspectie voltooid"
                },
                new Status()
                {
                    StatusName = "Afgerond"
                }
            };
            context.Statuses.AddRange(statuses);

            var customers = new List<Customer>
            {
                new Customer()
                {
                    Name = "BermDingetje",
                    City = "Den Haag",
                    IsDeleted = false
                },
                new Customer()
                {
                    Name = "Festispec",
                    City = "Den Bosch",
                    IsDeleted = false
                }
            };
            context.Customers.AddRange(customers);

            var contacts = new List<Contact>
            {
                new Contact()
                {
                    Name = "Darjush Kolahi",
                    PhoneNumber = "12345898765",
                    Email = "dkolahi@gmail.com",
                    Customer = customers[0],
                    IsDeleted = false
                },
                new Contact()
                {
                    Name = "Mitchell Appelman",
                    PhoneNumber = "678765356",
                    Email = "mappelman@gmail.com",
                    Customer = customers[1],
                    IsDeleted = false
                }
            };
            context.Contacts.AddRange(contacts);

            var events = new List<Event>
            {
                new Event()
                {
                    Name = "Pinkpop",
                    Address = "Megaland",
                    City = "Landgraaf",
                    Zipcode = "6372 XC",
                    Customer = customers[0],
                    IsDeleted = false
                },
                new Event()
                {
                    Name = "Appelpop",
                    Address = "Grasweide 15",
                    City = "Heusde",
                    Zipcode = "1234 AB",
                    Customer = customers[0],
                    IsDeleted = false
                },
                new Event()
                {
                    Name = "Zwarte Cross",
                    Address = "Zandweggetje 4",
                    City = "Lichtervoorde",
                    Zipcode = "2753 HG",
                    Customer = customers[1],
                    IsDeleted = false
                }
            };
            context.Events.AddRange(events);

            var inspectiondates = new List<InspectionDate>
            {
                new InspectionDate()
                {
                    StartDate = new DateTime(2018, 12, 1),
                    EndDate = new DateTime(2018, 12, 15),
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(2, 0, 0),
                    IsDeleted = false
                },
                new InspectionDate()
                {
                    StartDate = new DateTime(2018, 12, 20),
                    EndDate = new DateTime(2018, 12, 20),
                    StartTime = new TimeSpan(22, 0, 0),
                    EndTime = new TimeSpan(4, 0, 0),
                    IsDeleted = false
                }
            };
            context.InspectionDates.AddRange(inspectiondates);

            var inspections = new List<Inspection>
            {
                new Inspection()
                {
                    Name = "Zwarte Cross Inspectie",
                    Event = events[2],
                    Status = statuses[3],
                    Notes = "Minstens 4 inspecteurs",
                    InspectionDate = inspectiondates[0],
                    IsDeleted = false
                },
                new Inspection()
                {
                    Name = "Appelpop inspectie",
                    Event = events[1],
                    Status = statuses[3],
                    Notes = "",
                    InspectionDate = inspectiondates[1],
                    Inspectors = new ObservableCollection<Inspector>
                    {
                        inspectors[2],
                        inspectors[3]
                    },
                    IsDeleted = false
                }
            };
            context.Inspections.AddRange(inspections);

            var questionnaires = new List<Questionnaire>
            {
                new Questionnaire()
                {
                    Name = "Vragenlijst Zwarte Cross",
                    Instructions = "Lever de inspectie in 5 uur voor het einde van het festival.",
                    Version = 1,
                    Comments = "Drank gekocht tijdens het festival wordt niet door Festispec vergoed.",
                    Inspection = inspections[0],
                    IsDeleted = false
                },
                new Questionnaire()
                {
                    Name="Vragenlijst Appelpop",
                    Instructions="Niet te veel drinken.",
                    Version = 2,
                    Comments="Dit is niet onze eerste inspectie, maar toch goed opletten.",
                    Inspection = inspections[1],
                    IsDeleted=false
                }
            };
            context.Questionnaires.AddRange(questionnaires);

            var questions = new List<Question>
            {
                //zwarte cross
                new Question()
                {
                    Content = "Hoeveel man is er op het festival?",
                    Options = "A|100;B|200;C|500;D|1000",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content = "Hoe groot is het percentage van roest op het podium?",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Open Vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content = "Hoeveel klanten zijn er bij de bars?",
                    Questionnaire = questionnaires[0],
                    Columns="1|Grolsch bar| Bavaria bar| Hertog Jan Bar",
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Open Tabelvraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content = "Hoeveel bars zijn er op het festival?",
                    Options = "A|1-3;B|40-50;C|90;D|500+",
                    Columns="2|temp|voorbeeld|example",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice Tabelvraag"),
                    IsDeleted = false
                },
                //appelpop
                new Question()
                {
                    Content="Worden er band shirts verkocht",
                    Options="A|Jazeker;B|Nee;C|Misschien",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Multiple Choice vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Verkopen ze mooie shirts op het festival?",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Open Vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Verschillen tussen drankprijzen",
                    Columns="2|Grolsch bar|Bavaria Bar|Hertog Jan Bar",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Open Tabelvraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Hoe combineer je een multiple choice vraag met een tabelvraag?",
                    Options="A|Dat is onmogelijk;B|Dat doe je zo.",
                    Columns="1|Voorbeeld|Voorbeeld 2",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Multiple Choice Tabelvraag"),
                    IsDeleted = false
                },
            };
            context.Questions.AddRange(questions);

            var answers = new List<Answer>
            {
                new Answer()
                {
                    Content = "B|200",
                    Question = questions[0],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "A|100",
                    Question = questions[0],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|200",
                    Question = questions[0],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "C|500",
                    Question = questions[0],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "A|10%",
                    Question = questions[1],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "A|10%",
                    Question = questions[1],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "A|10%",
                    Question = questions[1],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|20%",
                    Question = questions[1],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "C|20",
                    Question = questions[3],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|10",
                    Question = questions[3],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "D|40",
                    Question = questions[3],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "C|20",
                    Question = questions[3],
                    Inspector = inspectors[3],
                    IsDeleted = false
                }
            };
            context.Answers.AddRange(answers);
        }
    }
}
