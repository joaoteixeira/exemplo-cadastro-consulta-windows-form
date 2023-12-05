using CpfCnpjLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApp1.Configuracao;
using WindowsFormsApp1.Modelos;
using WindowsFormsApp1.Uteis;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load1;
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            Consultar();
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            Funcionario _func = new Funcionario();

            _func.Nome = textBoxNome.Text;
            _func.CPF = textBoxCPF.Text;
            _func.DataNascimento = dateNasc.Value;

            if (ExistemTextBoxsVazios())
            {
                MessageBox.Show("Todos os campos são obrigatórios. Favor preencher os campos corretamente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!Cpf.Validar(_func.CPF))
            {
                MessageBox.Show("CPF Inválido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxCPF.Focus();
            }
            else
            {
                Inserir(_func);
            }
        }

        private void Inserir(Funcionario funcionario)
        {
            try
            {
                Conexao conexao = new Conexao();

                //Cadastro de Funcionario
                var comando = conexao.Comando("INSERT INTO funcionario (nome_func, cpf_func, datanasc_func) VALUES (@nome, @cpf, @data_nasc)");

                comando.Parameters.AddWithValue("@nome", funcionario.Nome);
                comando.Parameters.AddWithValue("@cpf", funcionario.CPF);
                comando.Parameters.AddWithValue("@data_nasc", funcionario.DataNascimento?.ToString("yyyy-MM-dd"));

                var resultado = comando.ExecuteNonQuery();

                if (resultado > 0)
                {
                    MessageBox.Show("Funcionário adicionado com sucesso!");
                }

                LimparTextBoxs();
                Consultar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void LimparTextBoxs()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox || control is MaskedTextBox)
                {
                    control.Text = String.Empty;
                }
            }
        }

        private bool ExistemTextBoxsVazios()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox || control is MaskedTextBox)
                {
                    var text = control.Text.Replace(",", "").Replace("-", "").Trim();

                    if (text == "")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Consultar()
        {
            List<Funcionario> listaFuncionarios = new List<Funcionario>();

            try
            {
                var conexao = new Conexao();

                var comando = conexao.Comando("SELECT * FROM funcionario");

                var leitor = comando.ExecuteReader();

                while (leitor.Read())
                {
                    var funcionario = new Funcionario();
                    funcionario.Id = DAOHelper.GetInt(leitor, "cod_func");
                    funcionario.Nome = DAOHelper.GetString(leitor, "nome_func");

                    funcionario.CPF = DAOHelper.GetString(leitor, "cpf_func");
                    funcionario.DataNascimento = DAOHelper.GetDateTime(leitor, "datanasc_func");


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
