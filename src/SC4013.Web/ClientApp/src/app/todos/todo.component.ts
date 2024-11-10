import {Component, TemplateRef, OnInit} from '@angular/core';
import {BsModalService, BsModalRef} from 'ngx-bootstrap/modal';
import { DatePipe } from '@angular/common'

import {
  TodoItemsClient, TodoItemDto,
  CreateTodoItemCommand, AssignTodoItemCommand,
  UpdateTodoItemCommand, UpdateDueDateCommand
} from '../web-api-client';

import {
  TodoListsClient, TodoListDto,
  CreateTodoListCommand, UpdateTodoListCommand
} from '../web-api-client';

@Component({
  selector: 'app-todo-component',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.scss']
})
export class TodoComponent implements OnInit {
  debug = false;
  lists: TodoListDto[];
  priorityLevels: PriorityLevel[];
  selectedList: TodoListDto;
  selectedItem: TodoItemDto;
  newListEditor: any = {};
  listOptionsEditor: any = {};
  itemDetailsEditor: any = {};
  newListModalRef: BsModalRef;
  listOptionsModalRef: BsModalRef;
  deleteListModalRef: BsModalRef;
  itemDetailsModalRef: BsModalRef;
  itemDueDateModalRef: BsModalRef;
  itemAssigneeModalRef: BsModalRef;

  constructor(
    private listsClient: TodoListsClient,
    private itemsClient: TodoItemsClient,
    private modalService: BsModalService,
    private datepipe: DatePipe
  ) {
    this.priorityLevels = [
      {id: 0, title: 'None'},
      {id: 1, title: 'Low'},
      {id: 2, title: 'Medium'},
      {id: 3, title: 'High'}
    ];
  }

  ngOnInit(): void {
    this.listsClient.getAllTodos().subscribe(
      result => {
        this.lists = result;
        if (this.lists.length) {
          this.selectedList = this.lists[0];
        }
      },
      error => console.error(error)
    );
  }

  // Lists
  remainingItems(list: TodoListDto): number {
    return list.items.filter(t => !t.done).length;
  }

  showNewListModal(template: TemplateRef<any>): void {
    this.newListModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('title').focus(), 250);
  }

  newListCancelled(): void {
    this.newListModalRef.hide();
    this.newListEditor = {};
  }

  addList(): void {
    const list = {
      id: 0,
      title: this.newListEditor.title,
      items: []
    } as TodoListDto;

    this.listsClient.createTodoList(list as CreateTodoListCommand).subscribe(
      result => {
        list.id = result;
        this.lists.push(list);
        this.selectedList = list;
        this.newListModalRef.hide();
        this.newListEditor = {};
      },
      error => {
        const errors = JSON.parse(error.response).errors;

        if (errors && errors.Title) {
          this.newListEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('title').focus(), 250);
      }
    );
  }

  showListOptionsModal(template: TemplateRef<any>) {
    this.listOptionsEditor = {
      id: this.selectedList.id,
      title: this.selectedList.title
    };

    this.listOptionsModalRef = this.modalService.show(template);
  }

  updateListOptions() {
    const list = this.listOptionsEditor as UpdateTodoListCommand;
    this.listsClient.updateTodoList(list).subscribe(
      () => {
        (this.selectedList.title = this.listOptionsEditor.title),
          this.listOptionsModalRef.hide();
        this.listOptionsEditor = {};
      },
      error => console.error(error)
    );
  }

  confirmDeleteList(template: TemplateRef<any>) {
    this.listOptionsModalRef.hide();
    this.deleteListModalRef = this.modalService.show(template);
  }

  deleteListConfirmed(): void {
    this.listsClient.deleteTodoList(this.selectedList.id).subscribe(
      () => {
        this.deleteListModalRef.hide();
        this.lists = this.lists.filter(t => t.id !== this.selectedList.id);
        this.selectedList = this.lists.length ? this.lists[0] : null;
      },
      error => console.error(error)
    );
  }

  // Items
  showItemDetailsModal(template: TemplateRef<any>, item: TodoItemDto): void {
    this.selectedItem = item;
    this.itemDetailsEditor = {
      ...this.selectedItem
    };

    this.itemDetailsEditor.dueDate = this.datepipe.transform(this.itemDetailsEditor.dueDate, 'yyyy-MM-dd');

    this.itemDetailsModalRef = this.modalService.show(template);
  }

  updateItemDetails(): void {
    const item = this.itemDetailsEditor as UpdateTodoItemCommand;
    this.itemsClient.updateTodoItem(item).subscribe(
      () => {
        if (this.selectedItem.listId !== this.itemDetailsEditor.listId) {
          this.selectedList.items = this.selectedList.items.filter(
            i => i.id !== this.selectedItem.id
          );
          const listIndex = this.lists.findIndex(
            l => l.id === this.itemDetailsEditor.listId
          );
          this.selectedItem.listId = this.itemDetailsEditor.listId;
          this.lists[listIndex].items.push(this.selectedItem);
        }

        this.selectedItem.description = this.itemDetailsEditor.description;
        this.selectedItem.priority = this.itemDetailsEditor.priority;
        this.itemDetailsModalRef.hide();
        this.itemDetailsEditor = {};
      },
      error => console.error(error)
    );
  }

  showItemDueDateModal(template: TemplateRef<any>, item: TodoItemDto): void {
    this.selectedItem = item;
    this.itemDetailsEditor = {
      ...this.selectedItem
    };

    this.itemDetailsEditor.dueDate = this.datepipe.transform(this.itemDetailsEditor.dueDate, 'yyyy-MM-dd');

    this.itemDueDateModalRef = this.modalService.show(template);
  }

  updateDueDate(): void {
    console.log(this.itemDetailsEditor);
    const item = new UpdateDueDateCommand();
    item.id = this.itemDetailsEditor.id;
    item.dueDate = new Date(this.itemDetailsEditor.dueDate);
    this.itemsClient.updateDueDate(item).subscribe(
      () => {
        if (this.selectedItem.listId !== this.itemDetailsEditor.listId) {
          this.selectedList.items = this.selectedList.items.filter(
            i => i.id !== this.selectedItem.id
          );
          const listIndex = this.lists.findIndex(
            l => l.id === this.itemDetailsEditor.listId
          );
          this.selectedItem.listId = this.itemDetailsEditor.listId;
          this.lists[listIndex].items.push(this.selectedItem);
        }

        this.selectedItem.dueDate = item.dueDate;
        this.itemDueDateModalRef.hide();
        this.itemDetailsEditor = {};
      },
      error => {
        if (error.status === 403) {
          alert('You are not authorized to update this item.');
        } else {
          alert('An error occurred while updating the item.');
        }
      }
    )
  }

  showItemAssigneeModal(template: TemplateRef<any>, item: TodoItemDto): void {
    this.selectedItem = item;
    this.itemDetailsEditor = {
      ...this.selectedItem
    };

    this.itemAssigneeModalRef = this.modalService.show(template);
  }

  updateAssignee():void {
    console.log(this.itemDetailsEditor);
    const item = new AssignTodoItemCommand();
    item.id = this.itemDetailsEditor.id;
    item.assigneeEmail = this.itemDetailsEditor.assigneeEmail;
    this.itemsClient.assignTodoItem(item).subscribe(
      () => {
        if (this.selectedItem.listId !== this.itemDetailsEditor.listId) {
          this.selectedList.items = this.selectedList.items.filter(
            i => i.id !== this.selectedItem.id
          );
          const listIndex = this.lists.findIndex(
            l => l.id === this.itemDetailsEditor.listId
          );
          this.selectedItem.listId = this.itemDetailsEditor.listId;
          this.lists[listIndex].items.push(this.selectedItem);
        }

        this.selectedItem.assigneeEmail = item.assigneeEmail;
        this.itemAssigneeModalRef.hide();
        this.itemDetailsEditor = {};
      },
      error => {
        if (error.status === 403) {
          alert('You are not authorized to update this item.');
        } else {
          alert('An error occurred while updating the item.');
        }
      }
    )
  }

  addItem() {
    const item = {
      id: 0,
      listId: this.selectedList.id,
      title: '',
      done: false
    } as TodoItemDto;

    this.selectedList.items.push(item);
    const index = this.selectedList.items.length - 1;
    this.editItem(item, 'itemTitle' + index);
  }

  editItem(item: TodoItemDto, inputId: string): void {
    this.selectedItem = item;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateItem(item: TodoItemDto, pressedEnter: boolean = false): void {
    const isNewItem = item.id === 0;

    if (!item.title.trim()) {
      this.deleteItem(item);
      return;
    }

    if (item.id === 0) {
      this.itemsClient
        .createTodoItem({title: item.title, listId: this.selectedList.id} as CreateTodoItemCommand)
        .subscribe(
          result => {
            item.id = result;
          },
          error => {
            if (error.status === 403) {
              alert('You are not authorized to add items to this list.');
            }else{
              alert('An error occurred while adding the item.');
            }
          }
        );
    } else {
      this.itemsClient.updateTodoItem(item as UpdateTodoItemCommand).subscribe(
        () => console.log('Update succeeded.'),
        error => {
          if (error.status === 403) {
            alert('You are not authorized to update this item.');
          } else {
            alert('An error occurred while updating the item.');
          }
        }
      );
    }

    this.selectedItem = null;

    if (isNewItem && pressedEnter) {
      setTimeout(() => this.addItem(), 250);
    }
  }

  deleteItem(item: TodoItemDto) {
    if (this.itemDetailsModalRef) {
      this.itemDetailsModalRef.hide();
    }

    if (item.id === 0) {
      const itemIndex = this.selectedList.items.indexOf(this.selectedItem);
      this.selectedList.items.splice(itemIndex, 1);
    } else {
      this.itemsClient.deleteTodoItem(item.id).subscribe(
        () =>
          (this.selectedList.items = this.selectedList.items.filter(
            t => t.id !== item.id
          )),
        error => {
          if (error.status === 403) {
            alert('You are not authorized to delete this item.');
          } else {
            alert('An error occurred while deleting the item.');
          }
        }
      );
    }
  }
}

export class PriorityLevel {
  id: number;
  title: string;
}
