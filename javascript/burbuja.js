function bubblesort(a)
{
    const s = a.length;

    // Iterando por todos los elementos del array
    for (let i = 0; i < s; i++) {
        let isSwapped = false;
        // Los últimos i elementos ya están en su lugar correspondiente.
        for (let j = 0; j < s - i - 1; j++) {
            // Intercambiando si el elemento encontrado es mayor que el siguiente
            if (a[j] > a[j + 1]) {
                [a[j], a[j + 1]] = [a[j + 1], a[j]];
                isSwapped = true;
            }
        }
        if (!isSwapped) break;
    }
}
//codigo del controlador para la prueba anterior
const a = [15, 16, 11, 13, 14];

console.log("Antes de ordenar los elementos del array son: ");
console.log(a.join(" "));

bubblesort(a);

console.log("Despues de ordenar los elementos del array son: ");
console.log(a.join(" "));