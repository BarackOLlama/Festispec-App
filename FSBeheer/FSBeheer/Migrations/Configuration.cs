namespace FSBeheer.Migrations
{
    using FSBeheer.Model;
    using System;
    using System.Collections.Generic;
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
            context.Inspectors.RemoveRange(context.Inspectors);
            context.Accounts.RemoveRange(context.Accounts);
            context.Roles.RemoveRange(context.Roles);
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('dbo.Roles', RESEED, 0)");
            context.Customers.RemoveRange(context.Customers);
            context.Events.RemoveRange(context.Events);

            var roles = new List<Role>
            {
                new Role() { Content = "Salesmedewerker" },
                new Role() { Content = "Operationele medewerker" },
                new Role() { Content = "Inspecteur" }
            };
            context.Roles.AddRange(roles);

            var questiontypes = new List<QuestionType>
            {
                new QuestionType() { Name = "Open vraag" },
                new QuestionType() { Name = "Multiple Choice vraag" },
                new QuestionType() { Name = "Open Tabelvraag" },
                new QuestionType() { Name = "Multiple Choice Tabelvraag" }
            };
            context.QuestionTypes.AddRange(questiontypes);

            var accounts = new List<Account>
            {
                new Account()
                {
                    Username = "bkoevoets@gmail.com",
                    Password = "bartswachtwoord",
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur")
                },
                new Account()
                {
                    Username = "pnguyen@gmail.com",
                    Password = "phiswachtwoord",
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur")
                },
                new Account()
                {
                    Username = "clancaster@gmail.com",
                    Password = "cjswachtwoord",
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur")
                },
                new Account()
                {
                    Username = "earends@gmail.com",
                    Password = "evertswachtwoord",
                    Role = roles.FirstOrDefault(r => r.Content == "Inspecteur")
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
                    Account = accounts[0]
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
                    Account = accounts[1]
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
                    Account = accounts[2]
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
                    Account = accounts[3]
                }
            };
            context.Inspectors.AddRange(inspectors);

            var inspections = new List<Inspection>
            {
                new Inspection()
                    {
                        Name = "EersteInspectie",
                        State = "Ingepland",
                        Notes = "Minstens 4 inspecteurs"
                    }
            };
            context.Inspections.AddRange(inspections);

            var questionnaires = new List<Questionnaire>
            {
                new Questionnaire()
                {
                    Name = "VragenlijstEersteInspectie",
                    Instructions = "Laat geen vragen leeg",
                    Version = 1,
                    Comments = "Dit is onze eerste inspectie, extra goed opletten!",
                    Inspection = inspections[0]
                }
            };
            context.Questionnaires.AddRange(questionnaires);

            var questions = new List<Question>
            {
                new Question()
                {
                    Content = "Hoeveel man is er op het festival?",
                    Options = "A|100;B|200;C|500;D|1000",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice vraag")
                },
                new Question()
                {
                    Content = "Hoe groot is het percentage van roest op het podium?",
                    Options = "A|10%;B|20%;C|50%;D|100%",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice vraag")
                },
                new Question()
                {
                    Content = "Wat is de draaglast van de fundament van het podium?",
                    Options = "A|100kg;B|200kg;C|500kg;D|1000kg",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice vraag")
                },
                new Question()
                {
                    Content = "Hoeveel bars zijn er op het festival?",
                    Options = "A|5;B|10;C|20;D|40",
                    Questionnaire = questionnaires[0],
                    QuestionType = questiontypes.FirstOrDefault(qt => qt.Name == "Multiple Choice vraag")
                }
            };
            context.Questions.AddRange(questions);

            var answers = new List<Answer>
            {
                new Answer()
                {
                    Content = "B|200",
                    Question = questions[0],
                    Inspector = inspectors[0]
                },
                new Answer()
                {
                    Content = "A|100",
                    Question = questions[0],
                    Inspector = inspectors[1]
                },
                new Answer()
                {
                    Content = "B|200",
                    Question = questions[0],
                    Inspector = inspectors[2]
                },
                new Answer()
                {
                    Content = "C|500",
                    Question = questions[0],
                    Inspector = inspectors[3]
                },
                new Answer()
                {
                    Content = "A|10%",
                    Question = questions[1],
                    Inspector = inspectors[0]
                },
                new Answer()
                {
                    Content = "A|10%",
                    Question = questions[1],
                    Inspector = inspectors[1]
                },
                new Answer()
                {
                    Content = "A|10%",
                    Question = questions[1],
                    Inspector = inspectors[2]
                },
                new Answer()
                {
                    Content = "B|20%",
                    Question = questions[1],
                    Inspector = inspectors[3]
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[0]
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[1]
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[2]
                },
                new Answer()
                {
                    Content = "B|200kg",
                    Question = questions[2],
                    Inspector = inspectors[3]
                },
                new Answer()
                {
                    Content = "C|20",
                    Question = questions[3],
                    Inspector = inspectors[0]
                },
                new Answer()
                {
                    Content = "B|10",
                    Question = questions[3],
                    Inspector = inspectors[1]
                },
                new Answer()
                {
                    Content = "D|40",
                    Question = questions[3],
                    Inspector = inspectors[2]
                },
                new Answer()
                {
                    Content = "C|20",
                    Question = questions[3],
                    Inspector = inspectors[3]
                }
            };
            context.Answers.AddRange(answers);

            var customers = new List<Customer>
            {
                new Customer()
                {
                    Name = "BermDingetje",
                    Place = "Den Haag"
                },
                new Customer()
                {
                    Name = "Festispec",
                    Place = "Den Bosch"
                }
            };
            context.Customers.AddRange(customers);

            var events = new List<Event>
            {
                new Event()
                {
                    Name = "Pinkpop",
                    Address = "Megaland",
                    City = "Landgraaf",
                    Customer = customers[1]
                }
            };
            context.Events.AddRange(events);
        }
    }
}
