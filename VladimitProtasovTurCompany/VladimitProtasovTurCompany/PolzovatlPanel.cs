using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace VladimitProtasovTurCompany
{
    public partial class PolzovatlPanel : Form
    {
        private Polzovatel polzovatel1;
        private Страны страны;
        private BDPolzovatel bdpolzovatel;
        private int последнееID;

        public PolzovatlPanel(Polzovatel polzovatel, BDPolzovatel bDPolzovatel,int id)
        {
            InitializeComponent();
            polzovatel1 = polzovatel;
            страны = new Страны();
            bdpolzovatel = bDPolzovatel;
            последнееID = id;
        }

        private void PolzovatlPanel_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now;

            for (int i = 0; i < страны.СтраныСписок.Length; i++)
            {
                comboBox1.Items.Add(страны.СтраныСписок[i].Название);
            }
            загрузитьТаблицу();
        }

        private void узнатьЦену()
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                Страна страна = страны.СтраныСписок[comboBox1.SelectedIndex];
                Отель отель = страна.Отели[comboBox2.SelectedIndex];
                стоимостьlabel.Text = ((страна.стоимость + отель.стоимость) + (int.Parse(comboBox3.Text)*100)).ToString() + " рублей.";
            }
            else
            {
                стоимостьlabel.Text = "неизвестно";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            Страна страна = страны.СтраныСписок[comboBox1.SelectedIndex];

            for (int i = 0; i < страна.Отели.Length; i++)
                comboBox2.Items.Add(страна.Отели[i].Название);
            водоемыlabel.Text = страна.наличиеВодоемов;
            походыlabel.Text = страна.турестическиеПоходы;
            спортивныесооружlabel.Text = страна.наличиеСпортивныхСооуржений;
            узнатьЦену();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            узнатьЦену();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            узнатьЦену();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                Страна страна = страны.СтраныСписок[comboBox1.SelectedIndex];
                Отель отель = страна.Отели[comboBox2.SelectedIndex];
                polzovatel1.ДобавитьПутевку(последнееID, страна.Название,отель.Название,стоимостьlabel.Text,comboBox3.Text,dateTimePicker1.Value.ToLongDateString(),страна.наличиеВодоемов,страна.турестическиеПоходы,страна.наличиеСпортивныхСооуржений);
                последнееID++;
                загрузитьТаблицу();
            }
            else
            {
                MessageBox.Show("Заполните все данные!");
            }
            bdpolzovatel.последнееID = последнееID;
            SerializeBDPolzovatel(bdpolzovatel);
        }

        private bool SerializeBDPolzovatel(BDPolzovatel bDPolzovatel)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BDPolzovatel));

            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, bDPolzovatel);
                ms.Seek(0, SeekOrigin.Begin);
                File.WriteAllText("polzovatel.xml", new StreamReader(ms).ReadToEnd());
            }
            return true;
        }

        private void загрузитьТаблицу()
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < polzovatel1.путевки.Count; i++)
            {
                Путевка путевка = polzovatel1.путевки[i];
                dataGridView1.Rows.Add(путевка.ID, путевка.расположение, путевка.название_отеля, путевка.стоимость, путевка.длительность,путевка.дата_Начала, путевка.наличие_водоемов, путевка.турестический_поход, путевка.наличие_спортивных_сооружений);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count >= 0)
            {
                Redactirovanie redactirovanie = new Redactirovanie(polzovatel1, страны,dataGridView1.CurrentCell.RowIndex);
                redactirovanie.ShowDialog();
                загрузитьТаблицу();
                SerializeBDPolzovatel(bdpolzovatel);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SerializeBDPolzovatel(bdpolzovatel))
            {
                Registraciya registraciya = new Registraciya();
                registraciya.Show();
                this.Hide();
            }
        }
    }

    public class Страны
    {
        private Страна[] страны;
        private string[] названияСтран;
        private Random random;

        public Страна[] СтраныСписок { get { return страны; } private set { } }

        public Страны()
        {
            названияСтран = new string[] { "Сочи", "Анапа", "Геленджик", "Кавказ", "Байкал"};
            страны = new Страна[названияСтран.Length];
            random = new Random();

            for (int i = 0; i < страны.Length; i++)
            {
                страны[i] = new Страна(названияСтран[i], random);
            }
        }
    }

    public class Страна
    {
        private Отель[] отели;
        private string название;
        public int стоимость;
        public string наличиеВодоемов { get; private set;}
        public string турестическиеПоходы { get; private set; }
        public string наличиеСпортивныхСооуржений { get; private set; }

        public Отель[] Отели { get { return отели; } private set { } }

        public string Название { get { return название; } private set { } }
        private string[] названиеОтелей;

        public Страна(string название, Random random)
        {
            названиеОтелей = new string[] { "Гамма", "Космос", "Марриотт", "Новотель", "Альфа", "Галло", "Лиготель", "Олимпия", "Кристофф", "Бест-отель", "Династия", "Аллегро", "Ладога" };
            Перемешать(ref названиеОтелей, random);
            this.название = название;
            отели = new Отель[5];

            for (int i = 0; i < отели.Length; i++)
                отели[i] = new Отель(random, названиеОтелей[i]);
            наличиеВодоемов = random.Next(0, 2) == 0 ? "есть" : "нету";
            турестическиеПоходы = random.Next(0, 2) == 0 ? "есть" : "нету";
            наличиеСпортивныхСооуржений = random.Next(0, 2) == 0 ? "есть" : "нету";
            стоимость = random.Next(2000,10000);
        }

        public void Перемешать<T>(ref T[] array, Random random)
        {
            for (int i = 0; i < array.Length-1; i++)
            {
                int randomIndex = random.Next(0, array.Length);
                T temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }
    }

    public class Отель
    {
        private string название;
        public int стоимость { get; private set; }

        public string Название { get { return название; } set { } }

        public Отель(Random random, string name)
        {
            название = name;
            стоимость = random.Next(3000,15000);
        }
    }
}