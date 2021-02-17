﻿using Business.Entities;
using Business.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.Web
{
    public partial class Cursos : System.Web.UI.Page
    {
        private CursoLogic _Logic;
        
        public CursoLogic Logic
        {
            get
            {
                if (_Logic == null)
                {
                    _Logic = new CursoLogic();
                }
                return _Logic;
            }
        }

        private Curso Entity { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadGrid();
            }
            if (this.gridView.Rows.Count > 0)
            {
                this.gridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private void LoadGrid()
        {
            MateriaLogic materia = new MateriaLogic();
            ComisionLogic comision = new ComisionLogic();

            this.gridView.DataSource = this.Logic.GetAll();
            this.gridView.DataBind();

            if (this.MateriaDropDown.Items.Count == 1)
            {
                this.MateriaDropDown.DataSource = materia.GetAll();
                this.MateriaDropDown.DataTextField = "Descripcion";
                this.MateriaDropDown.DataValueField = "ID";
                this.MateriaDropDown.DataBind();
            }
            if (this.ComisionDropDown.Items.Count == 1)
            {
                this.ComisionDropDown.DataSource = comision.GetAll();
                this.ComisionDropDown.DataTextField = "Descripcion";
                this.ComisionDropDown.DataValueField = "ID";
                this.ComisionDropDown.DataBind();
            }

        }

        public enum FormModes
        {
            alta,
            baja,
            modificacion
        }

        public FormModes FormMode
        {
            get { return (FormModes)this.ViewState["FormMode"]; }
            set { this.ViewState["FormMode"] = value; }
        }


        protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedID = (int)this.gridView.SelectedValue;
        }


        private int SelectedID
        {
            get
            {
                if (this.ViewState["SelectedID"] != null)
                {
                    return (int)this.ViewState["SelectedID"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["SelectedID"] = value;
            }
        }

        private bool IsEntitySelected
        {
            get
            {
                return (this.SelectedID != 0);
            }
        }


        private void LoadForm(int ID)
        {
            this.Entity = this.Logic.GetOne(ID);
            this.anioCalendarioTextBox.Text = this.Entity.AnioCalendario.ToString();
            this.cupoTextBox.Text = this.Entity.Cupo.ToString();
            this.MateriaDropDown.SelectedValue = this.Entity.Materia.ID.ToString();
            this.ComisionDropDown.SelectedValue = this.Entity.Comision.ID.ToString();

        }

        private void EnableForm(bool enable)
        {
            this.anioCalendarioTextBox.Enabled = enable;
            this.MateriaDropDown.Enabled = enable;
            this.ComisionDropDown.Enabled = enable;
            this.cupoTextBox.Enabled = enable;
        }

        protected void editarlinkButton_Click(object sender, EventArgs e)
        {
            if (this.IsEntitySelected)
            {
                this.EnableForm(true);
                this.formPanel.Visible = true;
                this.formActionPanel.Visible = true;
                this.FormMode = FormModes.modificacion;
                this.LoadForm(this.SelectedID);
            }
        }

        private void LoadEntity(Curso curso)
        {
            curso.AnioCalendario = int.Parse(this.anioCalendarioTextBox.Text);
            curso.Cupo = int.Parse(this.cupoTextBox.Text);

            curso.Comision = new Comision();
            curso.Materia = new Materia();
            
            curso.Comision.ID= int.Parse(this.ComisionDropDown.SelectedItem.Value);
            curso.Materia.ID = int.Parse(this.MateriaDropDown.SelectedItem.Value);

        }

        private void SaveEntity(Curso curso)
        {
            this.Logic.Save(curso);
        }

        private void DeleteEntity(int ID)
        {
            try
            {
                this.Logic.Delete(ID);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

        }

        protected void eliminarLinkButton_Click(object sender, EventArgs e)
        {
            if (this.IsEntitySelected)
            {
                this.formPanel.Visible = true;
                this.FormMode = FormModes.baja;
                this.formActionPanel.Visible = true;
                this.EnableForm(false);
                this.LoadForm(this.SelectedID);
            }
        }

        protected void nuevoLinkButton_Click(object sender, EventArgs e)
        {
            this.formPanel.Visible = true;
            this.formActionPanel.Visible = true;
            this.FormMode = FormModes.alta;
            this.ClearForm();
            this.EnableForm(true);
        }

        private void ClearForm()
        {
            this.anioCalendarioTextBox.Text = string.Empty;
            this.cupoTextBox.Text = string.Empty;

            this.MateriaDropDown.SelectedIndex = 0;
            this.ComisionDropDown.SelectedIndex = 0;
        }

        protected void aceptarLinkButton_Click(object sender, EventArgs e)
        {
            switch (this.FormMode)
            {
                case FormModes.baja:
                    this.DeleteEntity(this.SelectedID);
                    this.LoadGrid();
                    break;
                case FormModes.modificacion:
                    this.Entity = new Curso();
                    this.Entity.ID = this.SelectedID;
                    this.Entity.State = BusinessEntity.States.Modified;
                    this.LoadEntity(this.Entity);
                    this.SaveEntity(this.Entity);
                    this.LoadGrid();
                    break;
                case FormModes.alta:
                    this.Entity = new Curso();
                    this.LoadEntity(this.Entity);
                    this.SaveEntity(this.Entity);
                    this.LoadGrid();
                    break;
                default:
                    break;
            }
            this.formPanel.Visible = false;
            this.formActionPanel.Visible = false;

            this.gridView.SelectedIndex = -1;
            this.SelectedID = 0;
        }

        protected void cancelarLinkButtom_Click(object sender, EventArgs e)
        {
            this.formPanel.Visible = false;
            this.formActionPanel.Visible = false;

            this.gridView.SelectedIndex = -1;
            this.SelectedID = 0;
        }
    }
}