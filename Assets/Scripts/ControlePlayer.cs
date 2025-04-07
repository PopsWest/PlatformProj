using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ControlePlayer : MonoBehaviour
{
    float interval;
    [SerializeField]
    float aceleracao,forcaPulo,velocidadeMaxima;
    [SerializeField]
    LayerMask mascaraDeLayers;
    [SerializeField]
    Image barraPulo;
    public TextMeshProUGUI vidas, itens;
    bool noChao = false;
    bool jumping=false;
    Rigidbody2D rb;//referencia para o componente do Rigidbody2D
    int coletaItens = 0;
    int quantidadeVida = 3;
    InputAction move;
    InputAction jump;
    private void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
        interval = 1 * Time.deltaTime;
        


    }

    private void Update()
    {
        if (jump.WasPressedThisFrame() && noChao && barraPulo.fillAmount>0)
        {
            jumping = true;
            barraPulo.fillAmount -= 0.1f;
            
        }

        vidas.text = "Vidas: " + quantidadeVida.ToString();
        itens.text = "Pontos: " + coletaItens.ToString();
        
    }
    private void FixedUpdate()
    {
        vida();

        Vector2 direcao = move.ReadValue<Vector2>();
        if (direcao != Vector2.zero)
        {
            rb.AddForce(Vector2.right * direcao.x * aceleracao, ForceMode2D.Force);
            if (rb.linearVelocity.magnitude > velocidadeMaxima)
            {
                rb.linearVelocityX = velocidadeMaxima * direcao.x;
            }
        }
        else
        {
            rb.AddForce(new Vector2(rb.linearVelocityX * -aceleracao, 0), ForceMode2D.Force);
        }
        if (jumping)
        {
            rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
            jumping=false;
        }

       Vector2 baseObjeto = new Vector2(transform.position.x, 
            GetComponent<BoxCollider2D>().bounds.min.y);

        noChao = Physics2D.OverlapCircle(baseObjeto, 0.1f, mascaraDeLayers);

        if (barraPulo.fillAmount < 1)
        {

            StartCoroutine(Barra(interval));
        }
        


    }
    IEnumerator Barra(float interval)
    {
        
        
            
            yield return new WaitForSeconds(interval);
            barraPulo.fillAmount += 0.001f;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            coletaItens++;
            Destroy(collision.gameObject);
            
        }
    }

    void vida()
    {
        if(coletaItens%3==0)
        {
            quantidadeVida++;
        }
    }







    //Essa não é a melhor maneira de se checar 
    //se o Player está no chão
    private void OnCollisionStay2D(Collision2D collision)
    {
     //   noChao = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
     //   noChao = false;
    }

    
}
