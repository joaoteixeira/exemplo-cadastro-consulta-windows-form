using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Configuracao;
using WindowsFormsApp1.Modelos;
using WindowsFormsApp1.Uteis;

namespace WindowsFormsApp1
{
    public partial class FuncionarioConsulta : Form
    {
        private List<Funcionario> listaFuncionarios = new List<Funcionario>();

        public FuncionarioConsulta()
        {
            InitializeComponent();
        }


        private void Consultar()
        {
            try
            {
                var conexao = new Conexao();

                var comando = conexao.Comando("SELECT * FROM funcionario");

                var leitor = comando.ExecuteReader();

                while (leitor.Read())
                {
                    var funcionario = new Funcionario();
                    funcionario.Id = DAOHelper.GetInt(leitor, "id");
                    funcionario.Nome = DAOHelper.GetString(leitor, "nome_func");

                    funcionario.CPF = DAOHelper.GetString(leitor, "cpf_func");
                    funcionario.DataNascimento = DAOHelper.GetDateTime(leitor, "data_nasc_func");


                    listaFuncionarios.Add(funcionario);
                }

                dataGridViewFuncionario.DataSource = null;
                dataGridViewFuncionario.DataSource = listaFuncionarios;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
