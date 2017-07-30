# ObjectPool
Simple Object Pool for Unity 3D

# Usage

Initialization
```
  ObjectPool.GetInstance().Init();
```

Object registration
```
  ObjectPool.GetInstance().RegisterObject((string)[Path], (string)[Key], (int)[Initial Amount]);   
```

Use Object
```
  var go = ObjectPool.GetInstance().GetObject((string)[Key]);
```

Return Object
```
 ObjectPool.GetInstance().ReturnObject([GameObject]);
```
