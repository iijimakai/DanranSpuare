using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PunikonScroll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPushed = false; // �}�E�X��������Ă��邩������Ă��Ȃ���
    private Vector3 nowMousePos; // ���݂̃}�E�X�̃��[���h���W
    private Vector3 initialPosition; // �I�u�W�F�N�g�̏����ʒu
    float xLimit = 10.0f; // x�������̉��̈�
    float yLimit = 10.0f; // y�������̉��̈�

    void Start()
    {
        initialPosition = transform.position; // �I�u�W�F�N�g�̏����ʒu��ۑ�
    }

    void Update()
    {
        Vector3 nowMousePosi;
        Vector3 diffPos;
        Vector3 currentPos = transform.position;

        if (isPushed) {    // �}�E�X����������Ă��鎞�A�I�u�W�F�N�g�𓮂���
            nowMousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���݂̃}�E�X�̃��[���h���W���擾
            diffPos = (nowMousePosi - nowMousePos) * 3; // ��O�̃}�E�X���W�Ƃ̍������v�Z���ĕω��ʂ��擾
            diffPos.z = 0;    // z�����̂ݕω������Ȃ�
            transform.position += diffPos;  // �I�u�W�F�N�g�̍��W�Ƀ}�E�X�̕ω��ʂ𑫂��ĐV�������W��ݒ�
            nowMousePos = nowMousePosi; // ���݂̃}�E�X�̃��[���h���W���X�V
        }

        // �ʒu�̐���
        // currentPos.x = Mathf.Clamp(currentPos.x, initialPosition.x - xLimit, initialPosition.x + xLimit);
        // currentPos.y = Mathf.Clamp(currentPos.y, initialPosition.y - yLimit, initialPosition.y + yLimit);
        // transform.position = currentPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPushed = true; // �����J�n�@�t���O�𗧂Ă�
        nowMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // �}�E�X�̃��[���h���W��ۑ�
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPushed = false; // �����I���@�t���O�𗎂Ƃ�
        nowMousePos = Vector2.zero;
        transform.position = initialPosition; // �I�u�W�F�N�g�������ʒu�ɖ߂�
    }
}
