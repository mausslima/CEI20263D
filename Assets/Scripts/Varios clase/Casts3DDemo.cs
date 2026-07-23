using UnityEngine;
// Importamos el espacio de nombres UnityEngine, necesario para acceder a MonoBehaviour,
// Vector3, Color, Gizmos, Physics, etc.

public class Casts3DDemo : MonoBehaviour
// Definimos una clase pública llamada Casts3DDemo que hereda de MonoBehaviour,
// lo que permite que el script se adjunte a un GameObject en una escena de Unity 3D.
{
    [Header("Raycast")]
    // Encabezado "Raycast" en el inspector para agrupar las variables relacionadas con el raycast.

    public Vector3 rayOrigin = new Vector3(-3f, 2f, 0f);
    // Punto de origen del raycast en coordenadas del mundo (x, y, z).

    public Vector3 rayDirection = Vector3.right;
    // Dirección en la que se lanzará el raycast (en este caso, hacia el eje positivo X).

    public float rayDistance = 6f;
    // Distancia máxima que recorrerá el raycast desde su origen en la dirección indicada.

    [Header("SphereCast")]
    // Encabezado "SphereCast" para agrupar parámetros del sphere cast.

    public Vector3 sphereOrigin = new Vector3(-3f, 0f, 0f);
    // Origen desde el cual se empezará a proyectar el sphere cast.

    public float sphereRadius = 0.4f;
    // Radio de la esfera que se utilizará como volumen para el sphere cast.

    public Vector3 sphereDirection = Vector3.right;
    // Dirección en la que se desplazará la esfera durante el sphere cast.

    public float sphereDistance = 6f;
    // Distancia máxima que recorrerá el sphere cast desde su origen.

    [Header("BoxCast")]
    // Encabezado "BoxCast" para agrupar variables relacionadas con el box cast.

    public Vector3 boxOrigin = new Vector3(-3f, -2f, 0f);
    // Origen desde donde se proyectará el box cast.

    public Vector3 boxHalfExtents = new Vector3(0.4f, 0.4f, 0.4f);
    // Mitad de las dimensiones de la caja en cada eje (x, y, z).
    // Es decir, la caja completa tendrá tamaño 0.8x0.8x0.8.

    public Vector3 boxDirection = Vector3.right;
    // Dirección hacia la que se desplazará el volumen del box cast.

    public float boxDistance = 6f;
    // Distancia máxima que recorrerá el box cast desde su origen.

    public Quaternion boxRotation = Quaternion.identity;
    // Rotación de la caja representada como un Quaternion.
    // identity significa sin rotación inicial.

    [Header("Opcional")]
    // Encabezado "Opcional" para parámetros adicionales de configuración.

    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    // Máscara de capas que indica qué objetos pueden ser detectados por los distintos casts.

    public Color rayColor = Color.yellow;
    // Color con el que se dibujará el raycast en la escena (Debug.DrawRay y Gizmos).

    public Color sphereColor = Color.cyan;
    // Color usado para dibujar el sphere cast y su gizmo.

    public Color boxColor = Color.magenta;
    // Color usado para dibujar el box cast y sus gizmos.

    void Update()
    // Método de Unity que se ejecuta una vez por frame.
    // Aquí llamamos a los tres métodos de cast cada frame.
    {
        DoRaycast();
        // Ejecuta el raycast 3D y muestra su resultado.

        DoSphereCast();
        // Ejecuta el sphere cast 3D y muestra su resultado.

        DoBoxCast();
        // Ejecuta el box cast 3D y muestra su resultado.
    }

    void DoRaycast()
    // Método responsable de realizar el Raycast 3D y mostrar información del impacto.
    {
        RaycastHit hit;
        // Variable que almacenará información sobre el impacto del raycast
        // (punto de impacto, normal, collider, distancia, etc.).

        bool hasHit = Physics.Raycast(rayOrigin, rayDirection.normalized, out hit, rayDistance, layerMask);
        // Lanza un raycast desde rayOrigin, en la dirección normalizada rayDirection,
        // con una distancia máxima rayDistance y filtrando por layerMask.
        // Si hay impacto, devuelve true y rellena la estructura hit con información detallada.

        Debug.DrawRay(rayOrigin, rayDirection.normalized * rayDistance, rayColor);
        // Dibuja una línea en la ventana de Scene representando el raycast
        // desde rayOrigin hasta rayOrigin + rayDirection * rayDistance, con color rayColor.

        if (hasHit)
        // Comprobamos si el raycast ha impactado con algún collider.
        {
            Debug.Log($"[Raycast3D] Hit: {hit.collider.name} | Point: {hit.point} | Distance: {hit.distance}");
            // Mostramos en consola el nombre del collider impactado, el punto de impacto
            // y la distancia desde el origen del raycast hasta dicho punto.
        }
    }

    void DoSphereCast()
    // Método responsable de realizar el SphereCast 3D y mostrar información del impacto.
    {
        RaycastHit hit;
        // Estructura que almacenará los datos del impacto del sphere cast.

        bool hasHit = Physics.SphereCast(sphereOrigin, sphereRadius, sphereDirection.normalized, out hit, sphereDistance, layerMask);
        // Lanza un sphere cast: una esfera de radio sphereRadius que se proyecta desde sphereOrigin
        // en la dirección sphereDirection normalizada, recorriendo sphereDistance, filtrada por layerMask.
        // Devuelve true si detecta algún collider y rellena hit con la información correspondiente.

        Debug.DrawRay(sphereOrigin, sphereDirection.normalized * sphereDistance, sphereColor);
        // Dibuja una línea que representa el recorrido del sphere cast para visualizar su trayectoria.

        if (hasHit)
        // Si el sphere cast ha alcanzado algún collider...
        {
            Debug.Log($"[SphereCast3D] Hit: {hit.collider.name} | Point: {hit.point} | Distance: {hit.distance}");
            // Imprimimos en la consola el nombre del objeto impactado, el punto exacto de impacto
            // y la distancia desde el origen del sphere cast hasta ese punto.
        }
    }

    void DoBoxCast()
    // Método responsable de realizar el BoxCast 3D y mostrar información del impacto.
    {
        RaycastHit hit;
        // Estructura que almacenará los detalles del impacto del box cast.

        bool hasHit = Physics.BoxCast(
            boxOrigin,
            boxHalfExtents,
            boxDirection.normalized,
            out hit,
            boxRotation,
            boxDistance,
            layerMask
        );
        // Lanza un box cast: una caja de dimensiones 2 * boxHalfExtents, centrada en boxOrigin,
        // con rotación boxRotation, que se desplaza en la dirección boxDirection normalizada,
        // recorriendo boxDistance, y filtrando por layerMask.
        // Si detecta un collider, devuelve true y rellena la estructura hit.

        Debug.DrawRay(boxOrigin, boxDirection.normalized * boxDistance, boxColor);
        // Dibuja una línea indicando la trayectoria del box cast en la ventana de Scene.

        if (hasHit)
        // Si el box cast ha impactado con algún collider...
        {
            Debug.Log($"[BoxCast3D] Hit: {hit.collider.name} | Point: {hit.point} | Distance: {hit.distance}");
            // Mostramos en consola el nombre del objeto impactado, el punto de impacto
            // y la distancia desde el origen del box cast hasta dicho punto.
        }
    }

    void OnDrawGizmos()
    // Método especial de Unity que se llama para dibujar gizmos en la escena.
    // Se utiliza para representar visualmente información de depuración en el Editor.
    {
        // Raycast
        Gizmos.color = rayColor;
        // Establecemos el color de los gizmos al color asociado al raycast.

        DrawArrow(rayOrigin, rayDirection.normalized * rayDistance);
        // Dibujamos una flecha que indica el origen, dirección y distancia del raycast.

        // SphereCast
        Gizmos.color = sphereColor;
        // Cambiamos el color de los gizmos al color asociado al sphere cast.

        Gizmos.DrawWireSphere(sphereOrigin, sphereRadius);
        // Dibujamos una esfera alámbrica en el origen del sphere cast para mostrar su volumen inicial.

        DrawArrow(sphereOrigin, sphereDirection.normalized * sphereDistance);
        // Dibujamos una flecha que representa la trayectoria del sphere cast.

        // BoxCast
        Gizmos.color = boxColor;
        // Cambiamos el color de los gizmos al color asociado al box cast.

        Matrix4x4 oldMatrix = Gizmos.matrix;
        // Guardamos la matriz de transformación actual de los gizmos
        // para poder restaurarla después de hacer transformaciones específicas.

        Gizmos.matrix = Matrix4x4.TRS(boxOrigin, boxRotation, Vector3.one);
        // Asignamos una nueva matriz de transformación para los gizmos:
        // traslada hasta boxOrigin, aplica la rotación boxRotation y mantiene escala unitaria.

        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
        // Dibujamos una caja alámbrica centrada en el origen local (Vector3.zero),
        // con tamaño total 2 * boxHalfExtents (ancho, alto y profundo).

        Gizmos.matrix = oldMatrix;
        // Restauramos la matriz original para no afectar al resto de gizmos que se dibujen posteriormente.

        DrawArrow(boxOrigin, boxDirection.normalized * boxDistance);
        // Dibujamos una flecha que representa la trayectoria del box cast.
    }

    void DrawArrow(Vector3 start, Vector3 dir)
    // Método auxiliar para dibujar una flecha usando gizmos desde un punto de inicio start
    // siguiendo la dirección dir.
    {
        Vector3 end = start + dir;
        // Calculamos el punto final de la flecha sumando la dirección al origen.

        Gizmos.DrawLine(start, end);
        // Dibujamos la línea principal de la flecha entre start y end.

        // Flecha sencilla en 3D (plano XZ)
        Vector3 right = Quaternion.Euler(0, 25, 0) * (-dir.normalized) * 0.3f;
        // Calculamos uno de los lados de la punta de flecha:
        // rotamos la dirección inversa -dir.normalized 25 grados sobre el eje Y
        // y la escalamos para que tenga una longitud corta (0.3).

        Vector3 left = Quaternion.Euler(0, -25, 0) * (-dir.normalized) * 0.3f;
        // Calculamos el otro lado de la punta, rotando -dir.normalized -25 grados sobre el eje Y,
        // también con longitud 0.3.

        Gizmos.DrawLine(end, end + right);
        // Dibujamos el lado derecho de la punta de flecha desde el final de la línea principal.

        Gizmos.DrawLine(end, end + left);
        // Dibujamos el lado izquierdo de la punta de flecha desde el final de la línea principal.
    }
}