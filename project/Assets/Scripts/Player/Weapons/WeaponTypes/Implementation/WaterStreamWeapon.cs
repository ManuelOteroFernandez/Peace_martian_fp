using Unity.VisualScripting;
using UnityEngine;

public class WaterStreamWeapon : StreamWeapon
{
    LineRenderer lineRenderer;
    RaycastHit2D hit;
    [SerializeField] float lineLength = 10f;
    [SerializeField] float growSpeed = 15f;
    [SerializeField] float maxDuration = 5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject waterSplashParticlePrefab;
    [SerializeField] GameObject impactEffectPrefab;

    private float currentLength;
    private float currentDuration;
    private GameObject currentImpactEffect;


    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    //TODO: implementar cooldown?
    protected override void Update() {
        base.Update();

        if (isShooting) {
            CalculateBeamDuration();

            lineRenderer.enabled = true;

            //Se calcula el tamaño actual del rayo en función de la velocidad de crecimiento y la distancia máxima
            currentLength = Mathf.MoveTowards(currentLength, lineLength, growSpeed * Time.deltaTime);

            Vector3 firePointPosition = firePoint.position;
            hit = Physics2D.Raycast(firePointPosition, firePoint.right, currentLength, layerMask);

            // Convertir la posición a coordenadas locales del LineRenderer
            Vector3 localStart = transform.InverseTransformPoint(firePointPosition);
            Vector3 localEnd = hit ? transform.InverseTransformPoint(hit.point) 
                                : transform.InverseTransformPoint(firePointPosition + (Vector3)firePoint.right * currentLength);

            lineRenderer.SetPosition(0, localStart);
            lineRenderer.SetPosition(1, localEnd);

            if (hit) {
                Instantiate(waterSplashParticlePrefab, hit.point, Quaternion.identity);

                if (currentImpactEffect == null) {
                    currentImpactEffect = Instantiate(impactEffectPrefab, hit.point, firePoint.transform.rotation);
                } else {
                    currentImpactEffect.transform.position = hit.point;
                }
            } else {
                if (currentImpactEffect != null) {
                    Destroy(currentImpactEffect);
                    currentImpactEffect = null;
                }
            }


        } else {
            lineRenderer.enabled = false;
            currentLength = 0;
            currentDuration = 0;

            if (currentImpactEffect != null) {
                Destroy(currentImpactEffect);
                currentImpactEffect = null;
            }
        }
    }

    void CalculateBeamDuration() {
        currentDuration += Time.deltaTime;
        if (currentDuration >= maxDuration) {
            playerInputController.ForceRelease();
        }
    }

    protected override void OnStartShooting(){}

    protected override void OnStopShooting(){}
}
