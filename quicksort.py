def partition (a, l, h):
    #Selecciona el elemento pivote
    pvt = a[h]
    #j es el indice de los elementos que son menores que pivot y tambien inidica la posicion correcta del pivot encontrado hasta ese momento
    j = l - 1
    #Recorre a[l..h -1] y mueve todos los elementos menores
    #al lado izquierdo del pivote
    #elements to the left side
    #Los elementos de l a j son mas peuqueños despues de cada iteracion 
    for k in range (l, h): #Recorre el arreglo
        #Si el elemento actual es menor que el pivote
        if a[k] < pvt:
            j += 1 #incrementa el indice del elementos mas pequeño 
