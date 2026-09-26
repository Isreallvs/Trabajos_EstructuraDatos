def merge (a, l, m, r): #merge dos subarrays de a[]
    a1 = m - l + 1#tamaño del primer subarray
    a2 = r - m #Tamaño del segundo subarray
    #crear arrays temporales
    L = [0] * (a1) #Crea el array temporal
    R = [0] * (a2) #Crea el array temporal
    #Copiar datos a los arrays temporales L[] y R[]
    for j in range (0, a1):
        L[j] = a[l + j]
    for k in range (0, a2): #copiar datos al array temporal
        R[k] = a[m + 1 + k]
    i = 0 #indice inicial del primer subarray
    j = 0 #indice inicial del segundo subarray
    k = l #indice inicial del subarray mezclado

    #mezclar los arrays temporales de nuevo en a[l..r]
    while i < a1 and j < a2: #Recorrer ambos arrays
        if L[i] <= R[j]: #comparar los elementos de ambos arrays
            a[k] = L[i] #copiar el elemento mas pequeño al array original
            i = i + 1 #incrementar el indice del primer array
        else: #si el elemento del segundo array es mas pequeño
            a[k] = R[j] #Copiar el elemento mas pequeño del array original
            j = j + 1 #incrementar el indice del segundo array
        k = k + 1 #incrementar el indice del array original
    # copiar los elementos restantes de L[], si hay alguno 
    while i < a1: #copiar los elementos restantes del primer array
        a[k] = L[i] #copiar el elemento al array original
        i = i + 1 # incrementar el indice del primer array
        k = k + 1 # incrementar el indice del array original
# l es para el indice izquierdo y r es para el indice derecho del subarray de 'a' a ser ordenado
def mergeSort(a, l, r): #funcion principal que ordena a[l..r]
    if l < r:
        #igual que (l + r)//2. pero evita el desbordamiento para grandes valores de l y h 
        m = l + (r - l)//2
        #ordenar la primera y segunda mitad
        mergeSort(a, l, m) #ordenar la primera mitad
        mergeSort(a, m + 1, r) #ordenar la segunda mitad
        merge(a, l, m, r) #mezclar las dos mitades
#divide el array en dos mitades, las ordena y luego las mezcla
#codigo para probar la implementacion de MergeSort
a = [39, 28, 44, 11] #arreglo desordenado
s = len(a) #tamaño del arreglo
print ("Antes de ordenar el arreglo: ") #imprimir el arreglo
for j in range(s):
    print ("%d" % a[j], end=" ")
mergeSort(a, 0, s-1) #llama a la funcion mergesort
print ("\nDespues de ordenar el arreglo: ")
for j in range(s):
    print("%d" % a[j], end=" ")