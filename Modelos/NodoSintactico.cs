using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    public class NodoSintactico
    {
        private String nombre;
        private ArrayList hijos;
        private Object valor;
        private int numNodo;

        public NodoSintactico(String nombre, int numero)
        {
            this.nombre = nombre;
            hijos = new ArrayList();
            setNumNodo(numero);
        }

        public void addHijo(NodoSintactico hijo)
        {
            hijos.Add(hijo);
        }

        public String getNombre()
        {
            return nombre;
        }

        public void setNombre(String nombre)
        {
            this.nombre = nombre;
        }

        public ArrayList getHijos()
        {
            return hijos;
        }

        public void setHijos(ArrayList hijos)
        {
            this.hijos = hijos;
        }

        public Object getValor()
        {
            return valor;
        }

        public void setValor(Object valor)
        {
            this.valor = valor;
        }

        public int getNumNodo()
        {
            return numNodo;
        }

        public void setNumNodo(int numNodo)
        {
            this.numNodo = numNodo;
        }
    }
}
