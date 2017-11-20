using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest
{
    public class WorkDayCalculator : IWorkDayCalculator
    {   
        // старая(медленная) версия
        /*public Boolean IsWeekDays(DateTime startDate, WeekEnd[] weekEnds)
        {
            bool isWeekDay = true;

            for (int i = 0; i < weekEnds.Length; i++)
            {
                if (startDate >= weekEnds[i].StartDate &&
                    startDate <= weekEnds[i].EndDate)
                {
                    isWeekDay = false;
                }
            }
            return isWeekDay;
        }*/
        public int IndexBinarySearch(DateTime startDate, WeekEnd[] weekEnds)
        {
            int index = 0;
            int a = 0; // начало отрезка
            int b = weekEnds.Length - 1; // конец отрезка
            
            // пока длина отрезка [a,b] > 1 рубим его делением пополам
            while (b - a > 1)
            {
                index = (a + b) / 2; // целочисленное деление на 2

                if (startDate <= weekEnds[index].EndDate)
                {
                    // обрубаем правую часть отрезка [a, b]
                    b = index;
                }
                else if (startDate > weekEnds[index].EndDate)
                {
                    // обрубаем левую часть отрезка [a, b]
                    a = index;
                }
            }

            return index;
        }

        public DateTime Calculate(DateTime startDate, int dayCount, WeekEnd[] weekEnds)
        {
            //throw new NotImplementedException();
            //int count = 0;
            int day = 1;
            int lenght = 0;
            DateTime returnDate = startDate;
            try
            {
                lenght = weekEnds.Length;
            } catch { }

            if (lenght == 0)
            {
                returnDate = startDate.AddDays(dayCount);
            }
            else
            {
                weekEnds = weekEnds.OrderBy(a => a.StartDate).ToArray(); // сортируем массив по начальной дате

                // находим стартовую позицию в массиве
                int index = IndexBinarySearch(startDate, weekEnds);

                // если текущий день - выходной, то переставить на ближайший рабочий
                if (returnDate >= weekEnds[index].StartDate)
                {
                    returnDate = weekEnds[index].EndDate.AddDays(day);
                    ++index;
                }

                dayCount = dayCount - 1; // dayCount-1 - потому что не считаем начальную(стартовую) дату
                while (dayCount > 0) 
                {
                    // если мы за пределами массива(след. выходных уже нет, закончились элементы) ->
                    // добавляем столько сколько дней
                    if (index > weekEnds.Length - 1)
                    {
                        returnDate = returnDate.AddDays(dayCount);
                        break;
                    }
                    // сколько рабочих дней до след. выходного
                    int daysToNextWeekends = (weekEnds[index].StartDate - returnDate).Days;

                    // если кол-во дней которые надо добавить не достают до след. выходных - добавляем эти дни
                    if (dayCount < daysToNextWeekends)
                    {
                        returnDate = returnDate.AddDays(dayCount);
                        break;
                    }
                    // в противном случаи переводим дату на след. день после след. выходных ->
                    // -> и уменьшаем кол-во дней которые надо добавить
                    else
                    {
                        returnDate = weekEnds[index].EndDate.AddDays(day);
                        ++index;
                        dayCount -= daysToNextWeekends;
                    }
       
                }

                // старая(медленная) версия
                /* while (count < dayCount )
                 {
                     bool isWeekDay = IsWeekDays(startDate, weekEnds);
                     if ( isWeekDay == true)
                     {
                         returnDate = startDate;
                         count++;
                         if (count == dayCount) { break; }
                     }
                     startDate = startDate.AddDays(day);

                 }*/
            }

            return returnDate;
        }
    }
}
