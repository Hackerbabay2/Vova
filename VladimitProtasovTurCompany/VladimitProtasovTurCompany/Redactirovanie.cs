using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VladimitProtasovTurCompany
{
    public partial class Redactirovanie : Form
    {
        private Polzovatel Polzovatel;
        private Страны страны;
        private Путевка путевка;

        public Redactirovanie(Polzovatel polzovatel, Страны страны, int indexPutevki)
        {
            InitializeComponent();
            Polzovatel = polzovatel;
            this.страны = страны;
            this.путевка = polzovatel.путевки[indexPutevki];
        }

        private void Redactirovanie_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now;

            for (int i = 0; i < страны.СтраныСписок.Length; i++)
            {
                comboBox1.Items.Add(страны.СтраныСписок[i].Название);
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

        private void узнатьЦену()
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                Страна страна = страны.СтраныСписок[comboBox1.SelectedIndex];
                Отель отель = страна.Отели[comboBox2.SelectedIndex];
                стоимостьlabel.Text = ((страна.стоимость + отель.стоимость) + (int.Parse(comboBox3.Text) * 100)).ToString() + " рублей.";
            }
            else
            {
                стоимостьlabel.Text = "неизвестно";
            }
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
                путевка.обновить_Данные(путевка.ID, страна.Название, отель.Название, стоимостьlabel.Text, comboBox3.Text, dateTimePicker1.Value.ToLongDateString(), страна.наличиеВодоемов, страна.турестическиеПоходы, страна.наличиеСпортивныхСооуржений);
                this.Hide();
            }
        }
    }
}