﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Logic;
using Business.Entities;


namespace UI.Web
{
    public partial class DatosPersonales : System.Web.UI.Page
    {
        private PersonaLogic _logic;
        public PersonaLogic Logic
        {
            get
            {
                if (_logic == null)
                {
                    _logic = new PersonaLogic();
                }
                return _logic;
            }
        }

       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
               
                LoadPlan();
                LoadForm(PersonaActual.ID);
            }
            
            EnableForm(false);
        }

        private void LoadPlan()
        {
            PlanLogic plan = new PlanLogic();

           

            if (this.planDropDown.Items.Count == 1)
            {
                this.planDropDown.DataSource = plan.GetAll();
                this.planDropDown.DataTextField = "Descripcion";
                this.planDropDown.DataValueField = "ID";
                this.planDropDown.DataBind();
            }

        }

        private Persona _PersonaActual;

        public Persona  PersonaActual
        {
            get
            {
                if (_PersonaActual == null)
                {
                    _PersonaActual = (Persona)Session["USUARIO"]; ;
                }
                return _PersonaActual;
            }

            set
            {
                _PersonaActual = value;
            }
        }

        public enum FormModes
        {
            consulta,
            modificacion
        }

        public FormModes FormMode
        {
            get { return (FormModes)this.ViewState["FormMode"]; }
            set { this.ViewState["FormMode"] = value; }
        }
        
        private void LoadForm(int ID)
        {
            Persona persona = this.Logic.GetOneById(ID);
            this.nombreTextBox.Text = persona.Nombre;
            this.apellidoTextBox.Text = persona.Apellido;
            this.emailTextBox.Text = persona.Email;
            this.direccionTextBox.Text =persona.Direccion;
            this.telefonoTextBox.Text = persona.Telefono;
            this.fechaNacimientoTextBox.Text =persona.FechaNacimiento.ToShortDateString();
            this.legajoTextBox.Text = persona.Legajo.ToString();
            this.planDropDown.SelectedValue= persona.Plan.ID.ToString();

        }

        private void EnableForm(bool enable)
        {
            this.nombreTextBox.Enabled = enable;
            this.apellidoTextBox.Enabled = enable;
            this.emailTextBox.Enabled = enable;
            this.direccionTextBox.Enabled = enable;
            this.telefonoTextBox.Enabled = enable;
            this.fechaNacimientoTextBox.Enabled = enable;
            this.legajoTextBox.Enabled = enable;
            this.planDropDown.Enabled = enable;

         

        }





        private Persona LoadEntity()
        {
            Persona persona = new Persona();
            persona.ID = this.PersonaActual.ID;
            persona.Nombre = this.nombreTextBox.Text;
            persona.Apellido = this.apellidoTextBox.Text;
            persona.Email = this.emailTextBox.Text;
            persona.Direccion = this.direccionTextBox.Text;
            persona.TipoPersona = Persona.TipoPersonas.Alumno;
            persona.Legajo = int.Parse(this.legajoTextBox.Text);
            persona.FechaNacimiento = DateTime.Parse(this.fechaNacimientoTextBox.Text);
            persona.Telefono = this.telefonoTextBox.Text;
            persona.Plan = new Plan();
            persona.Plan.ID = int.Parse(this.planDropDown.SelectedItem.Value);

            return persona;
        }




        private void SaveEntity(Persona persona)
        {
            this.Logic.Save(persona);
        }


        protected void editarlinkButton_Click(object sender, EventArgs e)
        {
            
            
                this.EnableForm(true);
                this.FormMode = FormModes.modificacion;
                //this.LoadForm(AlumnoActual.ID);
            
        }

        protected void aceptarLinkButton_Click(object sender, EventArgs e)
        {
            switch (this.FormMode)
            {
                case FormModes.consulta:
                    break;
                case FormModes.modificacion:
                    
                    //this.AlumnoActual.State = BusinessEntity.States.Modified;
                    var persona = LoadEntity();
                    persona.State = BusinessEntity.States.Modified;
                    this.SaveEntity(persona);
                    this.PersonaActual = persona;
                    break;
                
                  
            }
            
        }

        protected void cancelarLinkButtom_Click(object sender, EventArgs e)
        {
            LoadForm(PersonaActual.ID);
            EnableForm(false);
        }

        
    }
}