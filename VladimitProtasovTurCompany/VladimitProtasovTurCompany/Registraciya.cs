using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace VladimitProtasovTurCompany
{

    public partial class Registraciya : Form
    {
        BDPolzovatel bdpz = new BDPolzovatel();
        private int id;

        public Registraciya()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            RegParolBox3.PasswordChar = '*';
            PovtoritRegParolBox5.PasswordChar = '*';
            VhodParolBox2.PasswordChar = '*';
        }

        private BDPolzovatel DeserializeBDPolzovatel()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BDPolzovatel));

            if (File.Exists("polzovatel.xml"))
            {
                using (FileStream fs = new FileStream("polzovatel.xml", FileMode.OpenOrCreate))
                {
                    bdpz = xmlSerializer.Deserialize(fs) as BDPolzovatel;
                }
            }
            id = bdpz.последнееID;
            return bdpz;
        }

        private void SerializeBDPolzovatel(BDPolzovatel bDPolzovatel)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BDPolzovatel));

            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, bDPolzovatel);
                ms.Seek(0,SeekOrigin.Begin);
                File.WriteAllText("polzovatel.xml", new StreamReader(ms).ReadToEnd());
            }
        }

        private void VhodVisiblebutton3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
        }

        private void RegistraciyVisibleabutton4_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            groupBox1.Visible = true;
        }

        private void Registraciyabutton2_Click(object sender, EventArgs e)
        {
            if (bdpz != null)
            {
                if (RegParolBox3.Text.Length >= 8)
                {
                    if (RegParolBox3.Text == PovtoritRegParolBox5.Text)
                    {
                        if (bdpz.проверитьЛогин(RegLoginBox4.Text))
                        {
                            MessageBox.Show("Этот логин занят!");
                        }
                        else if(bdpz.ProveritPolzovatelya(RegParolBox3.Text, PovtoritRegParolBox5.Text) == false)
                        {
                            bdpz.DobavitPolzovatel(RegLoginBox4.Text, PovtoritRegParolBox5.Text);
                            SerializeBDPolzovatel(bdpz);
                            RegLoginBox4.Text = "";
                            RegParolBox3.Text = "";
                            PovtoritRegParolBox5.Text = "";
                            MessageBox.Show("Вы зарегестрировались!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пароли не совпадают!");
                    }
                }
                else
                {
                    MessageBox.Show("Пароль должен быть не менее 8 символов!");
                }
            }
        }

        private void Vhodbutton1_Click(object sender, EventArgs e)
        {
            bdpz = DeserializeBDPolzovatel();

            if (bdpz != null)
            {
                if (bdpz.ProveritPolzovatelya(VhodLoginBox1.Text, VhodParolBox2.Text))
                {
                    PolzovatlPanel polzovatlPanel = new PolzovatlPanel(bdpz.PoluchitPolzivatel(VhodLoginBox1.Text, VhodParolBox2.Text), bdpz, id);
                    polzovatlPanel.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
        }

        private void Registraciya_Load(object sender, EventArgs e)
        {
            DeserializeBDPolzovatel();
        }
    }

    [Serializable]
    public class BDPolzovatel
    {
        public List<Polzovatel> polzovatels { get; set; } = new List<Polzovatel>();
        public int последнееID { get; set; }

        public BDPolzovatel() 
        {
            последнееID = 0;
        }

        public void DobavitPolzovatel(string login, string parol)
        {
            polzovatels.Add(new Polzovatel(login, parol));
        }

        public bool проверитьЛогин(string login)
        {
            for (int i = 0; i < polzovatels.Count; i++)
            {
                if (polzovatels[i].login == login)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ProveritPolzovatelya(string login, string parol)
        {
            for (int i = 0; i < polzovatels.Count; i++)
            {
                if (polzovatels[i].login == login && polzovatels[i].parol == parol)
                {
                    return true;
                }
            }
            return false;
        }

        public Polzovatel PoluchitPolzivatel(string login, string parol)
        {
            for (int i = 0; i < polzovatels.Count; i++)
            {
                if (polzovatels[i].login == login && polzovatels[i].parol == parol)
                {
                    return polzovatels[i];
                }
            }
            return null;
        }
    }

    [Serializable]
    public class Polzovatel
    {
        public string login { get; set; }
        public string parol { get; set; }
        public List<Путевка> путевки { get; set; }

        public Polzovatel() { }

        public Polzovatel(string login, string parol)
        {
            this.login = login;
            this.parol = parol;
            путевки = new List<Путевка>();
        }

        public void ДобавитьПутевку(int id, string расположение, string название_отеля, string стоимость, string длительность, string дата_начала, string наличие_водоемов, string турестический_поход, string наличие_спортивных_сооружений)
        {
            путевки.Add(new Путевка(id, расположение, название_отеля, стоимость, длительность, дата_начала, наличие_водоемов, турестический_поход, наличие_спортивных_сооружений));
        }
    }
}