def selection(a): #funcion para implementar el algoritmo de seleccion
    for i in range(len(a)): #recorre todo el arreglo
        small = i #indice del elemento mas pequeño
        for j in range(i+1, len(a)):#Encuentra el elemento mas pequeño en el arreglo
            if a[small] > a[j]: #Compara el elemento mas pequeño con el siguiente elemento
                small = j #actualiza el indice del elemento mas pequeño
            #intercambia el elemento mas pequeño con el primer elemento
            a[i], a[small] = a[small], a[i] #intercambia los elementos
def printArr(a): #funcion para imprimir el array
    for i in range(len(a)): #recorre todo el arreglo
        print (a[i], end = " ") #imprime el elemento
a = [65, 26, 13, 23, 12] #Arreglo desordenado
print ("Arreglo antes de ser ordenado: ")
printArr(a)
selection(a)
print("\nArreglo despues de ser ordenado: ")
selection(a)
printArr(a)