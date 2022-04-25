﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace CustomStructures.AVL_Tree
{
    /// <summary>
    /// Es un árbol avl 
    /// </summary>
    /// <typeparam name="T">Es el tipo de dato con el que vas a trabajar</typeparam>
    public class AVLTree<T> : Itree_AVL<T>
    {
        Func<T, T, int> CompareTo;
        Node<T> RootNode;
        public int Count;
        /// <summary>
        /// Inicializa el árbol
        /// </summary>
        /// <param name="CompareTo">Debes siempre enviarle una funcion que sea igual a CompareTo para poder trabajar el arbol</param>
        public AVLTree(Func<T, T, int> CompareTo)
        {
            this.CompareTo = CompareTo;
            RootNode = null;
            Count = 0;
        }
        private Node<T> FindParent(T value)
        {
            Node<T> nodoAnterior = null;
            Node<T> nodoActual = RootNode;

            while (nodoActual != null)
            {
                if (nodoActual.Value.Equals(value))
                {
                    return nodoAnterior;
                }
                else if (CompareTo(value, nodoActual.Value) > 0)
                {
                    nodoAnterior = nodoActual;
                    nodoActual = nodoActual.derecha;
                }
                else
                {
                    nodoAnterior = nodoActual;
                    nodoActual = nodoActual.izquierda;
                }
            }

            return null;
        }
        /// <summary>
        /// Funcion para buscar un valor
        /// </summary>
        /// <param name="value">El valor a buscar</param>
        /// <returns>El valor encontrado</returns>
        public T Find(T value)
        {
            Node<T> nodoActual = RootNode;

            while (nodoActual != null)
            {
                int Result = CompareTo(value, nodoActual.Value);
                if (Result < 0)
                {
                    nodoActual = nodoActual.izquierda;
                }
                else if (Result > 0)
                {
                    nodoActual = nodoActual.derecha;
                }
                else
                {
                    return nodoActual.Value;
                }

            }
            return default(T);
        }
        private Node<T> IFind(T Value)
        {
            Node<T> nodoActual = RootNode;

            while (nodoActual != null)
            {
                if (nodoActual.Value.Equals(Value))
                {
                    return nodoActual;
                }
                else if (CompareTo(Value, nodoActual.Value) < 0)
                {
                    nodoActual = nodoActual.derecha;
                }
                else
                {
                    nodoActual = nodoActual.izquierda;
                }
            }
            return null;
        }
        private Node<T> FindParent(Node<T> objetivo)
        {
            Node<T> nodoAnterior = null;
            Node<T> nodoActual = RootNode;

            while (nodoActual != null)
            {
                if (nodoActual.Equals(objetivo))
                {
                    return nodoAnterior;
                }
                else if (CompareTo(objetivo.Value, nodoActual.Value) > 0)
                {
                    nodoAnterior = nodoActual;
                    nodoActual = nodoActual.derecha;
                }
                else
                {
                    nodoAnterior = nodoActual;
                    nodoActual = nodoActual.izquierda;
                }
            }

            return null;
        }
        /// <summary>
        /// Añade un nuevo elemento al árbol
        /// </summary>
        /// <param name="Value">Valor a isertar</param>
        public void Add(T Value)
        {
            if (RootNode == null)
            {
                RootNode = new Node<T>();
                RootNode.Value = Value;
                Count++;
                return;
            }
            Count++;
            Add(Value, RootNode);
        }
        private void Add(T Value, Node<T> iterando)
        {
            int Result = CompareTo(Value, iterando.Value);
            if (Result < 0)
            {
                if (iterando.izquierda != null)
                {
                    Add(Value, iterando.izquierda);
                }
                else
                {
                    if (iterando.izquierda == null) { iterando.izquierda = new Node<T>(); }
                    iterando.izquierda.Value = Value;
                }
            }
            else if (Result > 0)
            {
                if (iterando.derecha != null)
                {
                    Add(Value, iterando.derecha);
                }
                else
                {
                    if (iterando.derecha == null) { iterando.derecha = new Node<T>(); }
                    iterando.derecha.Value = Value;
                }
            }

            while (Math.Abs(iterando.esBalancedo()) > 1)
            {
                if (iterando.esBalancedo() > 1)
                {
                    RotacionIzquierda(iterando);
                }
                else if (iterando.esBalancedo() < 1)
                {
                    RotacionDerecha(iterando);
                }
            }
        }
        private void RotacionIzquierda(Node<T> targetNode)
        {
            Node<T> parentNode = FindParent(targetNode);
            Node<T> newHead = targetNode.derecha;
            Node<T> tempHolder;

            if (newHead.derecha == null && newHead.izquierda != null)
            {
                RotacionDerecha(newHead);
                newHead = targetNode.derecha;
            }

            if (parentNode != null)
            {
                if (parentNode.derecha == targetNode)
                {
                    parentNode.derecha = newHead;
                }
                else
                {
                    parentNode.izquierda = newHead;
                }
            }
            else
            {
                RootNode = newHead;
            }
            tempHolder = newHead.izquierda;

            newHead.izquierda = targetNode;
            targetNode.derecha = tempHolder;
        }
        private void RotacionDerecha(Node<T> targetNode)
        {
            Node<T> parentNode = FindParent(targetNode);
            Node<T> newHead = targetNode.izquierda;
            Node<T> tempHolder;

            if (newHead.izquierda == null && newHead.derecha != null)
            {
                RotacionIzquierda(newHead);
                newHead = targetNode.izquierda;
            }

            if (parentNode != null)
            {
                if (parentNode.izquierda == targetNode)
                {
                    parentNode.izquierda = newHead;
                }
                else
                {
                    parentNode.derecha = newHead;
                }
            }
            else
            {
                RootNode = newHead;
            }
            tempHolder = newHead.derecha;

            newHead.derecha = targetNode;
            targetNode.izquierda = tempHolder;
        }
        /// <summary>
        /// Elimina un elemento del arbol
        /// </summary>
        /// <param name="targetValue">Valor a eliminar</param>
        /// <returns></returns>
        public bool Remove(T targetValue)
        {
            if (RootNode == null)
            {
                return false;
            }

            Node<T> targetNode = IFind(targetValue);
            if (targetNode == null)
            {
                return false;
            }
            Node<T> nodoActual = RootNode;

            Count--;
            Delete(targetNode, nodoActual);
            return true;
        }
        private void Delete(Node<T> targetNode, Node<T> nodoActual)
        {
            if (targetNode == nodoActual)
            {
                Node<T> LeftMax = targetNode.FindReplacement();
                Node<T> parentNode = FindParent(targetNode);

                if (LeftMax != null)
                {
                    Node<T> replacementNode = new Node<T>();
                    replacementNode.Value = LeftMax.Value;
                    replacementNode.izquierda = targetNode.izquierda;
                    replacementNode.derecha = targetNode.derecha;

                    if (targetNode != RootNode)
                    {
                        if (CompareTo(targetNode.Value, parentNode.Value) < 0)
                        {
                            parentNode.izquierda = replacementNode;
                        }
                        else
                        {
                            parentNode.derecha = replacementNode;
                        }
                    }
                    else
                    {
                        RootNode = replacementNode;
                    }
                    nodoActual = replacementNode;
                }
                else
                {
                    if (targetNode != RootNode)
                    {
                        if (CompareTo(targetNode.Value, parentNode.Value) < 0 || targetNode.Value.Equals(parentNode.Value))
                        {
                            parentNode.izquierda = targetNode.derecha;
                        }
                        else
                        {
                            parentNode.derecha = targetNode.derecha;
                        }
                    }
                    else
                    {
                        RootNode = targetNode.derecha;
                    }
                }

                if (LeftMax != null)
                {
                    Delete(LeftMax, nodoActual.izquierda);
                }
            }
            else if (CompareTo(nodoActual.Value, targetNode.Value) < 0)
            {
                Delete(targetNode, nodoActual.derecha);
            }
            else
            {
                Delete(targetNode, nodoActual.izquierda);
            }

            while (Math.Abs(nodoActual.esBalancedo()) > 1)
            {
                if (nodoActual.esBalancedo() > 1)
                {
                    RotacionIzquierda(nodoActual);
                }
                else
                {
                    RotacionDerecha(nodoActual);
                }
            }
        }
        /// <summary>
        /// Recorrido in order de los arboles 
        /// </summary>
        /// <returns>Una lista con los datos del arbol, esta lista es ordenada</returns>
        public List<T> InOrder()
        {
            List<T> returnList = new List<T>();
            InOrder(RootNode);

            return returnList;

            void InOrder(Node<T> startingNode)
            {
                if (startingNode.izquierda != null)
                {
                    InOrder(startingNode.izquierda);
                }
                returnList.Add(startingNode.Value);
                if (startingNode.derecha != null)
                {
                    InOrder(startingNode.derecha);
                }
            }
        }

    }
}
