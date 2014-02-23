using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace NewSecure
{
    public class DictionaryDB : HomePage
    {
        protected OleDbConnection connection;
        protected OleDbDataAdapter dbadapter;
        protected OleDbCommandBuilder commandbuilder;
        protected DataTable datatable;
        protected DataRowCollection row;
        protected DataSet dataset;

        protected void DataBase()
        {
            connection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|\\NewSecure2013.accdb"));// conenction to db

            dataset = new DataSet();

            string sql = String.Format("Select * From MainPieDictionary"); // sql query
            dbadapter = new OleDbDataAdapter(sql, connection); //execute query
            commandbuilder = new OleDbCommandBuilder(dbadapter); 
            dbadapter.Fill(dataset); // fill dataset with results
            datatable = dataset.Tables[0]; //initialize datatable with dataset tabke
            datatable.TableName = "MainPieDictionary"; //give datatable name
            row = datatable.Rows; //can throw away


        }

        public List<string> getDictionaryWords()
        {
            DataBase();
            List<string> DictWords = new List<string>();//create a empty list of string
            DataRow newRow;
            for (int i = 0; i < dataset.Tables["MainPieDictionary"].Rows.Count; i++)
            {
                newRow = dataset.Tables["MainPieDictionary"].Rows[i]; //retrieve the word 
                DictWords.Add(newRow["Terms"].ToString()); //add the word into the empty list 
            }
            return DictWords; //return the list
        }

        public string getWordCategory(string Term) //To match the tweets to the categories (Word)
        {
            DataRow[] returnedrows = datatable.Select("Terms ='" + Term + "'"); //retrieve datarow of the input term
            if (returnedrows != null)
                return returnedrows[0]["Category"].ToString(); // get the category of input term
            else
                return "";
        }

        public string getTweetCategory(string tweet) //To match the tweets to the categories (Pie Chart)
        {
            string category = "";
            var wordPattern = new Regex(@"\w+"); //regex to split phrases into word by word
            List<string> TermsList = getDictionaryWords(); // get the whole list of dict
            foreach (Match match in wordPattern.Matches(tweet)) //split tweets in word by word
            {
                for (int i = 0; i < TermsList.Count; i++)
                {
                    if (match.ToString().ToLower().Contains(TermsList[i].ToLower())) //if tweet word contain dict word
                    {
                        category = getWordCategory(TermsList[i]); //retrieve category of dict word
                    }
                }
            }
            return category;
        }

        public string getSecWordCategory(string Term)
        {
            DataRow[] returnedrows = datatable.Select("Terms ='" + Term + "'");
            if (returnedrows != null)
                return returnedrows[0]["SeCategory"].ToString();
            else
                return "";
        }

        public string getSecTweetCategory(string tweet) //To match the tweets to the categories (Pie Chart)
        {
            string category = "";
            var wordPattern = new Regex(@"\w+"); //regex to split phrases into word by word
            List<string> TermsList = getDictionaryWords(); // get the whole list of dict
            foreach (Match match in wordPattern.Matches(tweet)) //split tweets in word by word
            {
                for (int i = 0; i < TermsList.Count; i++)
                {
                    if (match.ToString().ToLower().Contains(TermsList[i].ToLower())) //if tweet word contain dict word
                    {
                        category = getSecWordCategory(TermsList[i]); //retrieve category of dict word
                    }
                }
            }
            return category;
        }
    }
}
