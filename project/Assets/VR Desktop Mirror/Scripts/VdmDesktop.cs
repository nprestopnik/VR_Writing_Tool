using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using UnityRawInput;

public class VdmDesktop : MonoBehaviour
{
    [HideInInspector]
    public int Screen = 0;
    [HideInInspector]
    public int ScreenIndex = 0;

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlags, int dx, int dy,
                      int dwData, int dwExtraInfo);

    [Flags]
    public enum MouseEventFlags
    {
        LEFTDOWN = 0x00000002,
        LEFTUP = 0x00000004,
        MIDDLEDOWN = 0x00000020,
        MIDDLEUP = 0x00000040,
        MOVE = 0x00000001,
        ABSOLUTE = 0x00008000,
        RIGHTDOWN = 0x00000008,
        RIGHTUP = 0x00000010
    }
    
    private VdmDesktopManager m_manager;
    private LineRenderer m_line;
    private Renderer m_renderer;
    private MeshCollider m_collider;

    private bool m_zoom = false;
    private bool m_zoomWithFollowCursor = false;

    private Vector3 m_positionNormal;
    private Quaternion m_rotationNormal;
    
    private float m_positionAnimationStart = 0;
    

    void Start()
    {
        m_manager = transform.parent.GetComponent<VdmDesktopManager>();
        m_line = GetComponent<LineRenderer>();
        m_renderer = GetComponent<Renderer>();
        m_collider = GetComponent<MeshCollider>();

        m_manager.Connect(this);

        Hide();
    }

    public void Update()
    {
        //print(isMousing);

        bool skip = false;
        if (Visible() == false)
            skip = true;

        if(skip == false)    
        {   
            float step = 0;
            if (Time.time - m_positionAnimationStart > 1)
                step = 1;
            else
                step = Time.time - m_positionAnimationStart;

            Vector3 positionDestination;
            Quaternion rotationDestination;
         
            positionDestination = m_positionNormal;
            rotationDestination = m_rotationNormal;


            if (transform.position != positionDestination)
                transform.position = Vector3.Lerp(transform.position, positionDestination, step);

            if (transform.rotation != rotationDestination)
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationDestination, step);
            
        }
    }
    
    void OnEnable()
    {
        m_manager = transform.parent.GetComponent<VdmDesktopManager>();
        m_manager.Connect(this);
		RawKeyInput.OnKeyDown += HandleKeyDown;
		RawKeyInput.OnKeyUp += HandleKeyUp;
    }

    void OnDisable()
    {
        m_manager.Disconnect(this);
    }

    public void HideLine()
    {
        m_line.enabled = false;
    }

    public void Hide()
    {
        m_renderer.enabled = false;
        m_collider.enabled = false;
    }

    public void Show()
    {
        m_renderer.enabled = true;
        m_collider.enabled = true;

        if(m_manager.EnableHackUnityBug)
        {
            m_manager.HackStart();
        }
    }

    public bool Visible()
    {
        return (m_renderer.enabled);
    }

    public void setDesktopTransform(Transform goal) {
        m_positionNormal = goal.position;
        m_rotationNormal = goal.rotation;
    }

    bool isMousing = false;

    //CUSTOM METHOD FOR CHECKING ANY RAYCAST
    //Used to check fingers from the Leap (Could be used for anything though)
    public void CheckRaycast(Vector3 rayOrigin, Vector3 rayDirection) {
        Vector3 origin;
        Vector3 direction;
        isMousing = false;

        origin = rayOrigin;
        direction = rayDirection;

        if(Visible()) {
            RaycastHit[] rcasts = Physics.RaycastAll(origin, direction);

            foreach (RaycastHit rcast in rcasts)
            {
                if (rcast.collider.gameObject != this.gameObject || rcast.distance > 1f) {
                    continue;
                } else {
                    isMousing = true; 
                }
                    
                if (m_manager.ShowLine)
                {
                    m_line.enabled = true;
                    m_line.SetPosition(0, origin);
                    m_line.SetPosition(1, rcast.point);
                }
                else
                {
                    m_line.enabled = false;
                }

                float dx = m_manager.GetScreenWidth(Screen);
                float dy = m_manager.GetScreenHeight(Screen);

                float vx = rcast.textureCoord.x;
                float vy = rcast.textureCoord.y;

                vy = 1 - vy;

                float x = (vx * dx);
                float y = (vy * dy);

                int iX = (int)x;
                int iY = (int)y;

                //m_manager.SetCursorPos(iX, iY);
            }
        }
    }

    //Using keys for mousepresses
    void HandleKeyDown(RawKey key) {
        if(isMousing) {
            print(key.ToString());
            if(key == RawKey.Numpad0) {
                print("LEFT CLICK");
                m_manager.SimulateMouseLeftDown();
                VdmDesktopManager.ActionInThisFrame = true;
            } else if(key == RawKey.Decimal) {
                print("RIGHT CLICK");
                m_manager.SimulateMouseRightDown();
                VdmDesktopManager.ActionInThisFrame = true;
            }
        }
	}

	//Event for Key Up
	void HandleKeyUp(RawKey key) {
        if(isMousing) {
            if(key == RawKey.Numpad0) {
                m_manager.SimulateMouseLeftUp();
                VdmDesktopManager.ActionInThisFrame = true;
            } else if(key == RawKey.Decimal) {
                m_manager.SimulateMouseRightUp();
                VdmDesktopManager.ActionInThisFrame = true;
            }
        }
	}

    public void ReInit(Texture2D tex, int width, int height)
    {
        GetComponent<Renderer>().material.mainTexture = tex;
        GetComponent<Renderer>().material.mainTexture.filterMode = m_manager.TextureFilterMode;
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

        float sx = width;
        float sy = height;
        sx = sx * m_manager.ScreenScaleFactor;
        sy = sy * m_manager.ScreenScaleFactor;
        transform.localScale = new Vector3(sx, sy, 1);
        
    }
    
}