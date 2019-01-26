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
            context.Quotations.RemoveRange(context.Quotations);
            context.Questions.RemoveRange(context.Questions);
            context.QuestionTypes.RemoveRange(context.QuestionTypes);
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('dbo.QuestionTypes', RESEED, 0)");
            context.Questionnaires.RemoveRange(context.Questionnaires);
            context.Inspections.RemoveRange(context.Inspections);
            context.InspectionDates.RemoveRange(context.InspectionDates);
            context.Inspectors.RemoveRange(context.Inspectors);
            context.ScheduleItems.RemoveRange(context.ScheduleItems);
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
                new QuestionType() { Name = "Multiple Choice Tabelvraag" },
                new QuestionType() { Name = "Schaal Vraag" }
            };
            context.QuestionTypes.AddRange(questiontypes);

            string bartSalt = BCrypt.GenerateSalt();
            string phiSalt = BCrypt.GenerateSalt();
            string cjSalt = BCrypt.GenerateSalt();
            string evertSalt = BCrypt.GenerateSalt();
            string mitchSalt = BCrypt.GenerateSalt();
            string sjakieSalt = BCrypt.GenerateSalt();

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
                },
                new Account()
                {
                    Username = "sjakie@festispec.com",
                    Password = BCrypt.HashPassword("password", sjakieSalt),
                    Role = roles.FirstOrDefault(e=> e.Content == "Inspecteur"),
                    Salt = sjakieSalt,
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

            var scheduleItems = new List<ScheduleItem>
            {
                new ScheduleItem()
                {
                    Inspector = inspectors[0],
                    Date = new DateTime(2018, 3, 1),
                    Scheduled = false,
                    IsDeleted = false
                },
                new ScheduleItem()
                {
                    Inspector = inspectors[0],
                    Date = new DateTime(2018, 3, 5),
                    Scheduled = true,
                    ScheduleStartTime = new TimeSpan(22, 0, 0),
                    ScheduleEndTime = new TimeSpan(3, 0, 0),
                    IsDeleted = false
                },
                new ScheduleItem()
                {
                    Inspector = inspectors[1],
                    Date = new DateTime(2018, 12, 3),
                    Scheduled = false,
                    IsDeleted = false
                },
                new ScheduleItem()
                {
                    Inspector = inspectors[1],
                    Date = new DateTime(2018, 11, 21),
                    Scheduled = true,
                    ScheduleStartTime = new TimeSpan(20, 0, 0),
                    ScheduleEndTime = new TimeSpan(23, 0, 0),
                    IsDeleted = false
                },
                new ScheduleItem()
                {
                    Inspector = inspectors[2],
                    Date = new DateTime(2018, 12, 20),
                    Scheduled = false,
                    IsDeleted = false
                },
                new ScheduleItem()
                {
                    Inspector = inspectors[3],
                    Date = new DateTime(2018, 12, 10),
                    Scheduled = false,
                    IsDeleted = false
                },
                new ScheduleItem()
                {
                    Inspector = inspectors[3],
                    Date = new DateTime(2018, 12, 5),
                    Scheduled = true,
                    ScheduleStartTime = new TimeSpan(20, 0, 0),
                    ScheduleEndTime = new TimeSpan(23, 0, 0),
                    IsDeleted = false
                }
            };
            context.ScheduleItems.AddRange(scheduleItems);

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
                    IsDeleted = false,
                    EventDate = new EventDate()
                    {
                        StartDate = new DateTime(2019, 1, 11),
                        EndDate = new DateTime(2019, 1, 15)
                    }
                },
                new Event()
                {
                    Name = "Appelpop",
                    Address = "Grasweide 15",
                    City = "Heusde",
                    Zipcode = "1234 AB",
                    Customer = customers[0],
                    IsDeleted = false,
                    EventDate = new EventDate()
                    {
                        StartDate = new DateTime(2019, 1, 16),
                        EndDate = new DateTime(2019, 1, 20)
                    }
                },
                new Event()
                {
                    Name = "Zwarte Cross",
                    Address = "Zandweggetje 4",
                    City = "Lichtervoorde",
                    Zipcode = "2753 HG",
                    Customer = customers[1],
                    IsDeleted = false,
                    EventDate = new EventDate()
                    {
                        StartDate = new DateTime(2019, 1, 21),
                        EndDate = new DateTime(2019, 1, 25)
                    }
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
                    Content = "Hoe is het weer?",
                    Options = "Slecht|1;Geweldig|10",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Schaal Vraag"),
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
                    Columns="2;Barnaam;Aantal klanten",
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Open Tabelvraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content = "Hoeveel soorten drank zijn er beschikbaar bij elke bar?",
                    Options = "A|1-3;B|40-50;C|90;D|500+",
                    Columns="2;Bar;Aantal soorten drank",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice Tabelvraag"),
                    IsDeleted = false
                },
                //appelpop
                new Question()
                {
                    Content ="Hoe is de stemming?",
                    Options="Grimmig|1;Gezellig|120",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Schaal Vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Worden er bandshirts verkocht",
                    Options="A|Jazeker;B|Nee;C|Misschien",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Multiple Choice vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Verkopen ze ook mooie shirts op het festival?",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Open Vraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Verschillen tussen drankprijzen",
                    Columns="2;Barnaam;Antwoord",
                    Questionnaire = questionnaires[1],
                    QuestionType = questiontypes.FirstOrDefault(e=> e.Name == "Open Tabelvraag"),
                    IsDeleted = false
                },
                new Question()
                {
                    Content="Hoe combineer je een multiple choice vraag met een tabelvraag?",
                    Options="A|Dat is onmogelijk;B|Dat doe je zo.",
                    Columns="2;Voorbeeld;Voorbeeld 2",
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
                    Content = "5",
                    Question = questions[1],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "2",
                    Question = questions[1],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "7",
                    Question = questions[1],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "9",
                    Question = questions[1],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "10%",
                    Question = questions[2],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "15%",
                    Question = questions[2],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "30%",
                    Question = questions[2],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "25%",
                    Question = questions[2],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Specialty Bar;50",
                    Question = questions[3],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Grolsche Bar;40",
                    Question = questions[3],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Heineken Bar;30",
                    Question = questions[3],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Pub;20",
                    Question = questions[3],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 1;A|1-3",
                    Question = questions[4],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 2;A|1-3",
                    Question = questions[4],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 1;B|40-50",
                    Question = questions[4],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 2;B|40-50",
                    Question = questions[4],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 1;C|90",
                    Question = questions[4],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 2;C|90",
                    Question = questions[4],
                    Inspector = inspectors[2],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 1;D|500+",
                    Question = questions[4],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Bar 2;D|500+",
                    Question = questions[4],
                    Inspector = inspectors[3],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "100",
                    Question = questions[5],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "110",
                    Question = questions[5],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "A|Jazeker",
                    Question = questions[6],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "B|Nee",
                    Question = questions[6],
                    Inspector = inspectors[1],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Dan doen ze ja",
                    Question = questions[7],
                    Inspector = inspectors[0],
                    IsDeleted = false
                },
                new Answer()
                {
                    Content = "Nee, dat doen ze niet",
                    Question = questions[7],
                    Inspector = inspectors[1],
                    IsDeleted = false
                }
            };
            context.Answers.AddRange(answers);
        }
    }
}
