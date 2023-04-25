using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.calitha.goldparser;
using System.IO;

namespace SourceCode
{
    public partial class Form1 : Form
    {
        MyParser p;
        MyParserclass parser;

        public Form1()
        {
            InitializeComponent();
            p = new MyParser("pgram.cgt",listBox1,listBox2);
            parser = new MyParserclass();
            parser.Setup();
            button1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            p.Parse(textBox1.Text);
            button1.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (parser.Parse(new StringReader (textBox1.Text)))
            {
                DrawReductionTree(parser.Root);
            }
            else
            {
                richTextBox1.Text = parser.ErrorMessage;
            }
        }

        void DrawReductionTree(GOLD.Reduction Root)
        {
            StringBuilder tree = new StringBuilder();
            tree.AppendLine("Root "+ Root.Parent.Text());
            DrawReduction(tree,Root,1);
            richTextBox1.Text = tree.ToString();
        }

        void DrawReduction(StringBuilder tree, GOLD.Reduction reduction, int indent)
        {
            int n;
            string indenttext = "";
            for(n=1;n<=indent;n++)
            {
                indenttext = "level" + n.ToString();
            }
            for(n=0;n<reduction.Count();n++)
            {
                switch(reduction[n].Type())
                {
                    case GOLD.SymbolType.Nonterminal:
                        GOLD.Reduction branch = (GOLD.Reduction)reduction[n].Data;
                        tree.AppendLine(indenttext + "      " + branch.Parent.Handle().Text());
                        DrawReduction(tree, branch, indent+1);

                        break;
                    default:
                        string leaf = (string)reduction[n].Data;
                        tree.AppendLine(indenttext + "      " + leaf);
                        break;
                }
            }
        }
    }
}
