using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace To_Do_List
{
    public partial class ToDoList : Form
    {

        DataTable todolist = new DataTable();
        bool isEditing = false;

        public ToDoList()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (todolist.Rows.Count > 0)
            {
                isEditing = true;
                panel2.BringToFront();
                Title.Text = todolist.Rows[TaskList.CurrentCell.RowIndex].ItemArray[0].ToString();
                Description.Text = todolist.Rows[TaskList.CurrentCell.RowIndex].ItemArray[1].ToString();
            }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            Title.Text = "";
            Description.Text = "";

            panel2.BringToFront();
        }

        private void ToDoList_Load(object sender, EventArgs e)
        {
            todolist.Columns.Add("Title");
            todolist.Columns.Add("Description");
            TaskList.DataSource = todolist;

            LoadData();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            isEditing = false;
            panel2.SendToBack();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try{
                todolist.Rows[TaskList.CurrentCell.RowIndex].Delete();
            }
            catch(Exception exc){
                Console.WriteLine("Error " + exc);
            }
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                todolist.Rows[TaskList.CurrentCell.RowIndex]["Title"] = Title.Text;
                todolist.Rows[TaskList.CurrentCell.RowIndex]["Description"] = Description.Text;
            }
            else
            {
                todolist.Rows.Add(Title.Text, Description.Text);
            }

            Title.Text = "";
            Description.Text = "";
            isEditing = false;

            panel2.SendToBack();
        }

        private void SaveData()
        {
            var tasklist = todolist.Rows.OfType<DataRow>().Select(
                row => new
                {
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString()
                }
            ).ToList();

            string jsonString = JsonSerializer.Serialize(tasklist);
            File.WriteAllText("todolist.json", jsonString);
        }

        private void LoadData()
        {
            if(File.Exists("todolist.json"))
            {
                string jsonString = File.ReadAllText("todolist.json");
                var tasklist = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonString);

                foreach(var task in tasklist)
                {
                    DataRow row = todolist.NewRow();
                    row["Title"] = task["Title"];
                    row["Description"] = task["Description"];
                    todolist.Rows.Add(row);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveData();
            base.OnFormClosing(e);
        }
    }
}
