# Application.Service.Image

```mermaid
    graph TB    
        subgraph List["**List** (ImageListFullRequest)"]
            direction TB
            start1((start))
            Image1[List of RepositryImages]
            termination1((end))

            %%{relationship}%%
            start1 --"List"--> Image1 --> termination1
        end

        subgraph List2["**List** (ImageListWithTagRequest)"]
            direction TB
            start2((start))
            Image2[RepositryImage]
            termination2((end))

            %%{relationship}%%
            start2 --"List"--> Image2 --> termination2
        end

```

---

```mermaid
---
config:
    class:
        hideEmptyMembersBox: true
---
    classDiagram

        class AppRequest {
            <<abstract>>
        }

        class AppResponse {
            <<abstract>>
            + Success: bool
            + ErrorMessage: string
        }
    
        class ImageListFullRequest {
        }

        class ImageListWithTagRequest {
        }

        class ImageListFullResponse{
            + Images: IList~RepositryImage~
        }

        class ImageListWithTagResponse{
            + Image: RepositryImage
        }

        %%{relationship}%%
        AppRequest <|-- ImageListFullRequest
        AppResponse <|--ImageListFullResponse
        AppRequest <|-- ImageListWithTagRequest
        AppResponse <|--ImageListWithTagResponse

```
