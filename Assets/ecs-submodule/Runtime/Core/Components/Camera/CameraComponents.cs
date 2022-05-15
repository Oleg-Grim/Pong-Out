﻿namespace ME.ECS.Camera {
    
    public struct Camera : IComponent, IVersioned {

        public bool perspective;
        public float orthoSize;
        public float fieldOfView;
        public float aspect;
        public float nearClipPlane;
        public float farClipPlane;

    }

}