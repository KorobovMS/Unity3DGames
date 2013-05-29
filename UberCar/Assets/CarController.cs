using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    
    public WheelCollider[] WColForward;
    public WheelCollider[] WColBack;
    
    public Transform[] wheelsF; //1
    public Transform[] wheelsB; //1
    
    public float wheelOffset = 0.1f; //2
    public float wheelRadius = 0.13f; //2
    
    public float maxSteer = 1;
    public float maxAccel = 1;
    public float maxBrake = 1;
    
    public Transform COM;
    
    public class WheelData{ //3
        public Transform wheelTransform; //4
        public WheelCollider col; //5
        public Vector3 wheelStartPos; //6 
        public float rotation = 0.0f;  //7
    }
    
    protected WheelData[] wheels; //8
    
    // Use this for initialization
    
        
    void Start () {
        rigidbody.centerOfMass = COM.localPosition;
        
        wheels = new WheelData[WColForward.Length+WColBack.Length]; //8
        
        for (int i = 0; i<WColForward.Length; i++){ //9
            wheels[i] = SetupWheels(wheelsF[i],WColForward[i]); //9
        }
        
        for (int i = 0; i<WColBack.Length; i++){ //9
            wheels[i+WColForward.Length] = SetupWheels(wheelsB[i],WColBack[i]); //9
        }
        
    }
    
    
    private WheelData SetupWheels(Transform wheel, WheelCollider col){ //10
        WheelData result = new WheelData(); 
        
        result.wheelTransform = wheel; //10
        result.col = col; //10
        result.wheelStartPos = wheel.transform.localPosition; //10
        
        return result; //10
        
    }
    
    void FixedUpdate () {
        
        float accel = 0;
        float steer = 0;
                
        accel = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");		
        
        CarMove(accel,steer);
        UpdateWheels(); //11
    }
    
    
    private void UpdateWheels(){ //11
        float delta = Time.fixedDeltaTime; //12
        
        
        foreach (WheelData w in wheels){ //13
            WheelHit hit; //14
                                
            Vector3 lp = w.wheelTransform.localPosition; //15
            if(w.col.GetGroundHit(out hit)){ //16
                lp.y -= Vector3.Dot(w.wheelTransform.position - hit.point, transform.up) - wheelRadius; //17
            }else{ //18
                
                lp.y = w.wheelStartPos.y - wheelOffset; //18
            }
            w.wheelTransform.localPosition = lp; //19
            
            
            w.rotation = Mathf.Repeat(w.rotation + delta * w.col.rpm * 360.0f / 60.0f, 360.0f); //20
            w.wheelTransform.localRotation = Quaternion.Euler(w.rotation, w.col.steerAngle, 90.0f); //21
        }	
        
    }
    
    private void CarMove(float accel,float steer){
        
        foreach(WheelCollider col in WColForward){
            col.steerAngle = steer*maxSteer;
        }
        
        if(accel == 0){
            foreach(WheelCollider col in WColBack){
                col.brakeTorque = maxBrake;
            }	
            
        }else{
                                
            foreach(WheelCollider col in WColBack){
                col.brakeTorque = 0;
                col.motorTorque	= accel*maxAccel;
            }	
            
        }
        
                
        
    }
    
}