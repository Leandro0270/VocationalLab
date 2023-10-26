using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskHeightController : MonoBehaviour
{
    [SerializeField] private float alturaInicial;
    [SerializeField] private float alturaMaxima;
    [SerializeField] private float alturaMinima;
    private bool _subindo;
    private bool _descendo;
    


    // Update is called once per frame
    void Update()
    {
        if (_subindo)
        {
            if(transform.position.y < alturaMaxima)
                transform.position += new Vector3(0, 0.001f, 0);
        }else if (_descendo)
        {
            if(transform.position.y > alturaMinima)
                transform.position -= new Vector3(0, 0.001f, 0);
        }
    }


    public void SubirMesa(bool subindo)
    {
        _subindo = subindo;
    }

    public void DescerMesa(bool descer)
    {
        _descendo = descer;
    }
    
    public void ResetarMesa()
    {
        transform.position = new Vector3(transform.position.x, alturaInicial, transform.position.z);
    }
}
