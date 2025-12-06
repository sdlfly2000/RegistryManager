# RegistryManager
## Intro
It is a simple cli controller to manage image repositories in private docker [registry](https://hub.docker.com/_/registry). Now, it supports list image, tag, and remove tag on image from registry.

## Usage
### List all Images from Registry
```
RegistryCtl image ls --all
```

### List all tags on image from Registry
```
RegistryCtl image ls --name <image name> 
```

### Remove tags on image from Registry
```
RegistryCtl image rm --name <image name> --tag <tag name> 
```

### Do not forget to garbage collect in Registry (like, setup a cron job below)
```
# Clean up Private Docker Registry at 3 am every day

* 3 * * * kubectl exec pod/docker-registry-0 -n docker-registry -- bin/registry garbage-collect /etc/distribution/config.yml
```