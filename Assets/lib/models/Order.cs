using System;
using System.Collections.Generic;
using Ceras;

namespace Sesim.Models
{
    public partial class Order
    {
        //订单名字
        public string name;

        //工程的完成难度，随着公司名气的提高可以接到更难的订单
        public int degree_of_difficulty;
 
        //工程的报酬，定金以及没有按时完成的违约金
        public decimal reward;
        public decimal deposit;
        public decimal breach_of_contract;

        //适合的语言可以加快进度
        public string language;
        //public string methods;
        //先不设定使用的方法

        public int time;
        //完成工程的限定时间
        public int quantities;
        //根据工程的等级以及工程的大小安排工程的工作量
        public int Number_of_employees;
        //限定完成订单的人数
    }
}
