using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VladimitProtasovTurCompany
{
    [Serializable]
    public class Путевка
    {
        public int ID { get; set; }
        public string расположение { get; set; }
        public string название_отеля { get; set; }
        public string стоимость { get; set; }
        public string длительность { get; set; }
        public string дата_Начала { get; set; }
        public string наличие_водоемов { get; set; }
        public string турестический_поход { get; set; }
        public string наличие_спортивных_сооружений { get; set; }

        public Путевка()
        {
        }

        public Путевка(int id,string расположение, string название_отеля, string стоимость, string длительность, string дата_начала, string наличие_водоемов, string турестический_поход, string наличие_спортивных_сооружений)
        {
            this.ID = id;
            this.расположение = расположение;
            this.название_отеля = название_отеля;
            this.стоимость = стоимость;
            this.длительность = длительность;
            this.дата_Начала = дата_начала;
            this.наличие_водоемов = наличие_водоемов;
            this.турестический_поход = турестический_поход;
            this.наличие_спортивных_сооружений = наличие_спортивных_сооружений;
        }

        public void обновить_Данные(int id,string расположение, string название_отеля, string стоимость, string длительность, string дата_начала, string наличие_водоемов, string турестический_поход, string наличие_спортивных_сооружений)
        {
            this.ID = id;
            this.расположение = расположение;
            this.название_отеля = название_отеля;
            this.стоимость = стоимость;
            this.длительность = длительность;
            this.дата_Начала = дата_начала;
            this.наличие_водоемов = наличие_водоемов;
            this.турестический_поход = турестический_поход;
            this.наличие_спортивных_сооружений = наличие_спортивных_сооружений;
        }
    }
}
