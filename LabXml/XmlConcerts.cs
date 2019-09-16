using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace LabXml
{
    public partial class XmlConcerts : Form
    {
        interface IAnalizatorXMLStrategy
        {
            List<Concerts> Search(Concerts concert);
        }

        class AnalizatorXMLDOMStrategy : IAnalizatorXMLStrategy
        {
            public List<Concerts> Search(Concerts concert)
            {
                List<Concerts> result = new List<Concerts>();
                XmlDocument doc = new XmlDocument();
                doc.Load(@"D:\Програмування\C#\LabXml\LabXml\Concerts.xml");

                XmlNode node = doc.DocumentElement;
                foreach(XmlNode nod in node.ChildNodes)
                {
                    string Artist = "";
                    string Date = "";
                    string PriceRange = "";
                    string Location = "";
                    string Style = "";
                    string ArtistBand = "";

                    foreach(XmlAttribute attribute in nod.Attributes)
                    {
                        if (attribute.Name.Equals("Artist") && (attribute.Value.Equals(concert.Artist) || concert.Artist.Equals(String.Empty)))
                            Artist = attribute.Value;

                        if (attribute.Name.Equals("Date") && (attribute.Value.Equals(concert.Date) || concert.Date.Equals(String.Empty)))
                            Date = attribute.Value;

                        if (attribute.Name.Equals("PriceRange") && (isPriceRange(attribute.Value, concert.PriceRange) || concert.PriceRange.Equals(String.Empty)))
                            PriceRange = attribute.Value;

                        if (attribute.Name.Equals("Location") && (attribute.Value.Contains(concert.Location) || concert.Location.Equals(String.Empty)))
                            Location = attribute.Value;

                        if (attribute.Name.Equals("Style") && (attribute.Value.Equals(concert.Style) || concert.Style.Equals(String.Empty)))
                            Style = attribute.Value;

                        if (attribute.Name.Equals("ArtistBand") && (attribute.Value.Equals(concert.ArtistBand) || concert.ArtistBand.Equals(String.Empty)))
                            ArtistBand = attribute.Value;
                    }

                    if (Artist != "" && Date != "" && PriceRange != "" && Location != "" && Style != "" && ArtistBand != "")
                    {
                        Concerts myConcert = new Concerts();
                        myConcert.Artist = Artist;
                        myConcert.Date = Date;
                        myConcert.PriceRange = PriceRange;
                        myConcert.Location = Location;
                        myConcert.Style = Style;
                        myConcert.ArtistBand = ArtistBand;

                        result.Add(myConcert);
                    }
                }
                return result;
            }
        }

        class AnalizatorXMLSAXStrategy : IAnalizatorXMLStrategy
        {
            public List<Concerts> Search(Concerts concert)
            {
                List<Concerts> AllResult = new List<Concerts>();
                var xmlReader = new XmlTextReader(@"D:\Програмування\C#\LabXml\LabXml\Concerts.xml");

                while (xmlReader.Read())
                {
                    if (xmlReader.HasAttributes)
                    {
                        while (xmlReader.MoveToNextAttribute())
                        {
                            string Artist = "";
                            string Date = "";
                            string PriceRange = "";
                            string Location = "";
                            string Style = "";
                            string ArtistBand = "";

                            if (xmlReader.Name.Equals("Artist") && (xmlReader.Value.Equals(concert.Artist) || concert.Artist.Equals(String.Empty)))
                            {
                                Artist = xmlReader.Value;

                                xmlReader.MoveToNextAttribute();

                                if (xmlReader.Name.Equals("Date") && (xmlReader.Value.Equals(concert.Date) || concert.Date.Equals(String.Empty)))
                                {
                                    Date = xmlReader.Value;

                                    xmlReader.MoveToNextAttribute();

                                    if (xmlReader.Name.Equals("PriceRange") && ((isPriceRange(xmlReader.Value, concert.PriceRange) || concert.PriceRange.Equals(String.Empty))))
                                    {
                                        PriceRange = xmlReader.Value;

                                        xmlReader.MoveToNextAttribute();

                                        if (xmlReader.Name.Equals("Location") && (xmlReader.Value.Contains(concert.Location)) || concert.Location.Equals(String.Empty))
                                        {
                                            Location = xmlReader.Value;

                                            xmlReader.MoveToNextAttribute();

                                            if (xmlReader.Name.Equals("Style") && (xmlReader.Value.Equals(concert.Style) || concert.Style.Equals(String.Empty)))
                                            {
                                                Style = xmlReader.Value;

                                                xmlReader.MoveToNextAttribute();

                                                if (xmlReader.Name.Equals("ArtistBand") && (xmlReader.Value.Equals(concert.ArtistBand) || concert.ArtistBand.Equals(String.Empty)))
                                                {
                                                    ArtistBand = xmlReader.Value;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (Artist != "" && Date != "" && PriceRange != "" && Location != "" && Style != "" && ArtistBand != "")
                            {
                                Concerts myConcert = new Concerts();
                                myConcert.Artist = Artist;
                                myConcert.Date = Date;
                                myConcert.PriceRange = PriceRange;
                                myConcert.Location = Location;
                                myConcert.Style = Style;
                                myConcert.ArtistBand = ArtistBand;

                                AllResult.Add(myConcert);
                            }
                        }
                    }
                }

                xmlReader.Close();
                return AllResult;
            }
        }

        class AnalizatorXMLLINQStrategy : IAnalizatorXMLStrategy
        {
            public List<Concerts> Search(Concerts concert)
            {
                List<Concerts> allResult = new List<Concerts>();
                var doc = XDocument.Load(@"D:\Програмування\C#\LabXml\LabXml\Concerts.xml");
                var result = from obj in doc.Descendants("Concert")
                             where
                             (
                             (obj.Attribute("Artist").Value.Equals(concert.Artist) || concert.Artist.Equals(String.Empty)) &&
                             (obj.Attribute("Date").Value.Equals(concert.Date) || concert.Date.Equals(String.Empty)) &&
                             (isPriceRange(obj.Attribute("PriceRange").Value, concert.PriceRange)|| concert.PriceRange.Equals(String.Empty)) &&
                             (obj.Attribute("Location").Value.Contains(concert.Location) || concert.Location.Equals(String.Empty)) &&
                             (obj.Attribute("Style").Value.Equals(concert.Style) || concert.Style.Equals(String.Empty)) &&
                             (obj.Attribute("ArtistBand").Value.Equals(concert.ArtistBand) || concert.ArtistBand.Equals(String.Empty))
                             )
                             select new
                             {
                                 artist = (string)obj.Attribute("Artist"),
                                 date = (string)obj.Attribute("Date"),
                                 priceRange = (string)obj.Attribute("PriceRange"),
                                 location = (string)obj.Attribute("Location"),
                                 style = (string)obj.Attribute("Style"),
                                 artistBand = (string)obj.Attribute("ArtistBand")
                             };
                foreach (var n in result)
                {
                    Concerts myConcert = new Concerts();
                    myConcert.Artist = n.artist;
                    myConcert.Date = n.date;
                    myConcert.PriceRange = n.priceRange;
                    myConcert.Location = n.location;
                    myConcert.Style = n.style;
                    myConcert.ArtistBand = n.artistBand;

                    allResult.Add(myConcert);
                }

                return allResult;
            }
        }

        class Search
        {
            public Concerts concert;

            public Search(IAnalizatorXMLStrategy analizator, Concerts conc)
            {
                Strategy = analizator;
                concert = conc;
            }

            public IAnalizatorXMLStrategy Strategy { private get; set; }

            public List<Concerts> SearchAlgorithm()
            {
                return Strategy.Search(concert);
            }
        }

        class Concerts
        {
            public Concerts()
            {
                Artist = string.Empty;
                Date = string.Empty;
                PriceRange = string.Empty;
                Location = string.Empty;
                Style = string.Empty;
                ArtistBand = string.Empty;
            }

            public string Artist { get; set; }

            public string Date { get; set; }

            public string PriceRange { get; set; }

            public string Location { get; set; }

            public string Style { get; set; }

            public string ArtistBand { get; set; }
        }

        public XmlConcerts()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllConcerts();
        }

        public void GetAllConcerts()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\Програмування\C#\LabXml\LabXml\Concerts.xml");

            XmlElement xRoot = doc.DocumentElement;
            XmlNodeList childNodes = xRoot.SelectNodes("Concert");

            for (int i = 0; i < childNodes.Count; i++)
            {
                XmlNode n = childNodes.Item(i);
                addItems(n);
            }
            string cheapPrice = "0 - 700";
            string mediumPrice = "700 - 2500";
            string highPrice = "2500 - 5000";
            string veryHighPrice = "5000 - 10000";

            comboBoxPriceRange.Items.Add(cheapPrice);
            comboBoxPriceRange.Items.Add(mediumPrice);
            comboBoxPriceRange.Items.Add(highPrice);
            comboBoxPriceRange.Items.Add(veryHighPrice);

            comboBoxLocation.Items.Add("Київ");
            comboBoxLocation.Items.Add("Львів");
            comboBoxLocation.Items.Add("Одеса");
            comboBoxLocation.Items.Add("Чернівці");
        }

        private void addItems(XmlNode n)
        {
            if (!comboBoxArtist.Items.Contains(n.SelectSingleNode("@Artist").Value))
                comboBoxArtist.Items.Add(n.SelectSingleNode("@Artist").Value);

            if (!comboBoxDate.Items.Contains(n.SelectSingleNode("@Date").Value))
                comboBoxDate.Items.Add(n.SelectSingleNode("@Date").Value);

            if (!comboBoxStyle.Items.Contains(n.SelectSingleNode("@Style").Value))
                comboBoxStyle.Items.Add(n.SelectSingleNode("@Style").Value);

            if (!comboBoxArtistBand.Items.Contains(n.SelectSingleNode("@ArtistBand").Value))
                comboBoxArtistBand.Items.Add(n.SelectSingleNode("@ArtistBand").Value);
        }

        static private bool isPriceRange(string value, string priceRange)
        {
            string firstValue = getPrice(ref value);
            string lastValue = getPrice(ref value);

            string firstPriceRange = getPrice(ref priceRange);
            string lastPriceRange = getPrice(ref priceRange);

            if (firstValue == " " || lastPriceRange == " ")
                return false;

            if (Convert.ToInt32(firstValue) <= Convert.ToInt32(lastPriceRange)) //&& Convert.ToInt32(lastPriceRange) >= Convert.ToInt32(lastValue))
                return true;
            else
                return false;
        }

        static private string getPrice(ref string line)
        {
            try
            {
                int i = 0;
                string price = "";

                while (line[i] != '-')
                {
                    price += line[i];
                    if (i + 1 == line.Length)
                        break;
                    i++;
                }

                if (i != line.Length)
                    line = line.Remove(0, i + 1);
                else
                    line = line.Remove(0, i);
                return price;
            }
            catch { return " "; }
        }

        private void buttonTransform_Click(object sender, EventArgs e)
        {
            transform();
        }

        private void transform()
        {
            XslCompiledTransform xct = new XslCompiledTransform();
            xct.Load(@"D:\Програмування\C#\LabXml\LabXml\Concerts.xsl");
            string fXML = @"D:\Програмування\C#\LabXml\LabXml\Concerts.xml";
            string fHTML = @"D:\Програмування\C#\LabXml\LabXml\Concerts.html";
            xct.Transform(fXML, fHTML);
        }

        private void Results_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            Results.Text = "";
            Concerts concert = new Concerts();

            if (checkArtist.Checked)
                concert.Artist = comboBoxArtist.SelectedItem.ToString();

            if (checkDate.Checked)
                concert.Date = comboBoxDate.SelectedItem.ToString();

            if (checkPriceRange.Checked)
                concert.PriceRange = comboBoxPriceRange.SelectedItem.ToString();

            if (checkLocation.Checked)
                concert.Location = comboBoxLocation.SelectedItem.ToString();

            if (checkStyle.Checked)
                concert.Style = comboBoxStyle.SelectedItem.ToString();

            if (checkArtistBand.Checked)
                concert.ArtistBand = comboBoxArtistBand.SelectedItem.ToString();

            IAnalizatorXMLStrategy analizator = new AnalizatorXMLDOMStrategy(); // по замовчуванню

            if (radioButtonDOM.Checked)
                analizator = new AnalizatorXMLDOMStrategy();

            if (radioButtonSAX.Checked)
                analizator = new AnalizatorXMLSAXStrategy();

            if (radioButtonLINQ.Checked)
                analizator = new AnalizatorXMLLINQStrategy();

            Search search = new Search(analizator, concert);
            List<Concerts> results = search.SearchAlgorithm();

            foreach (Concerts conc in results)
            {
                Results.Text += "Артист: " + conc.Artist + "\n";
                Results.Text += "Дата: " + conc.Date + "\n";
                Results.Text += "Цінова категорія: " + conc.PriceRange + "\n";
                Results.Text += "Місце проведення: " + conc.Location + "\n";
                Results.Text += "Жанр музики: " + conc.Style + "\n";
                Results.Text += "Артист чи група: " + conc.ArtistBand + "\n";

                Results.Text += "\n\n\n";
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Results.Text = "";
        }
    }
}
